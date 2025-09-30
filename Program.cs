using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Aimbot
{
    public class Program
    {
        [DllImport("kernel32.dll")] public static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_SHOW = 5;

        public static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            if (handle != IntPtr.Zero)
            {
                ShowWindow(handle, SW_SHOW);
            }

            try
            {
                dllmain.EntryPoint();
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[+] Fatal error: {ex.Message}");
                Console.WriteLine("[+] Press any key to exit...");
                Console.ReadKey();
            }
        }
    }

    public class dllmain
    {
        static string CurrentUserId = null;
        static string CurrentUsername = null;
        static DateTime LastAuth = DateTime.MinValue;
        static readonly TimeSpan SessionTimeout = TimeSpan.FromMinutes(30);

        public static KeyAuth.api KeyAuthApp = new KeyAuth.api(
            name: "STREAMER",
            ownerid: "xAX9Qn1kjg",
            version: "3.0"
        );

        public static void EntryPoint()
        {
            KeyAuthApp.init();
            try
            {
                ConsoleLogin();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[+] Error: {ex.Message}");
                Console.WriteLine("[+] Press any key to exit...");
                Console.ReadKey();
                throw;
            }
        }

        static void ConsoleLogin()
        {
            while (true)
            {
                Console.Write(" [+] Enter Username: ");
                string username = Console.ReadLine();
                Console.Write(" [+] Enter Password: ");
                string password = Console.ReadLine();

                try
                {
                    KeyAuthApp.login(username, password);
                    if (KeyAuthApp.response.success)
                    {
                        Console.WriteLine(" [+] Login success!");
                        CurrentUserId = KeyAuthApp.user_data.username;
                        CurrentUsername = username;
                        LastAuth = DateTime.UtcNow;
                        var handle = Program.GetConsoleWindow();
                        if (handle != IntPtr.Zero)
                        {
                            Program.ShowWindow(handle, 0); 
                        }
                        ClearCommandHistory();
                        StartHttpServer();
                        break;
                    }
                    else
                    {
                        Console.WriteLine($" [+] Login failed: {KeyAuthApp.response.message}");
                        Console.WriteLine(" [+] Press any key to retry...");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" [+] Login error: {ex.Message}");
                    Console.WriteLine(" [+] Press any key to retry...");
                    Console.ReadKey();
                }
            }
        }

        static void ClearCommandHistory()
        {
            try
            {
                ProcessStartInfo psInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "-Command Clear-History",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                try
                {
                    using (var psProcess = System.Diagnostics.Process.Start(psInfo))
                    {
                        psProcess?.WaitForExit();
                    }
                }
                catch
                {

                }

                string doskeyHistory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "commandhistory.txt");
                try
                {
                    if (File.Exists(doskeyHistory))
                    {
                        File.Delete(doskeyHistory);
                    }
                }
                catch
                {

                }
            }
            catch
            {

            }
        }

        static void StartHttpServer()
        {
            int port = 20919;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    var listener = new HttpListener();
                    listener.Prefixes.Add($"http://+:{port}/");
                    listener.Start();
                    while (true)
                    {
                        var ctx = listener.GetContext();
                        HandleHttpRequest(ctx);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"HTTP server error: {ex.Message}");
                }
            });
        }

        static string ReadEmbedded(string name)
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream(name))
            {
                if (stream == null) return null;
                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }

        static void HandleHttpRequest(HttpListenerContext ctx)
        {
            var req = ctx.Request;
            var resp = ctx.Response;

            try
            {
                resp.AddHeader("Access-Control-Allow-Origin", "*");
                resp.AddHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                resp.AddHeader("Access-Control-Allow-Headers", "Content-Type");

                if (req.HttpMethod == "OPTIONS")
                {
                    resp.StatusCode = 204;
                    resp.Close();
                    return;
                }

                string path = req.Url.AbsolutePath.TrimStart('/').ToLower();

                if (req.HttpMethod == "GET")
                {
                    string resName = null;
                    if (path == "" || path == "main" || path == "home")
                        resName = "Aimbot.Properties.main.html";

                    if (!string.IsNullOrEmpty(resName))
                    {
                        string html = ReadEmbedded(resName);
                        if (html == null)
                        {
                            resp.StatusCode = 404;
                            byte[] nbuf = Encoding.UTF8.GetBytes("404 Not Found");
                            resp.OutputStream.Write(nbuf, 0, nbuf.Length);
                        }
                        else
                        {
                            byte[] buf = Encoding.UTF8.GetBytes(html);
                            resp.ContentType = "text/html";
                            resp.ContentLength64 = buf.Length;
                            resp.OutputStream.Write(buf, 0, buf.Length);
                        }
                    }
                    else
                    {
                        resp.StatusCode = 404;
                        byte[] nbuf = Encoding.UTF8.GetBytes("404 Not Found");
                        resp.OutputStream.Write(nbuf, 0, nbuf.Length);
                    }
                    resp.OutputStream.Close();
                    return;
                }

                if (req.HttpMethod == "POST" && path == "session")
                {
                    bool active = CurrentUserId != null && (DateTime.UtcNow - LastAuth) < SessionTimeout;
                    var respObj = new { active, username = CurrentUserId };
                    byte[] buf = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(respObj));
                    resp.ContentType = "application/json";
                    resp.OutputStream.Write(buf, 0, buf.Length);
                    resp.OutputStream.Close();
                    return;
                }

                resp.StatusCode = 404;
                byte[] errorBuf = Encoding.UTF8.GetBytes("404 Not Found");
                resp.OutputStream.Write(errorBuf, 0, errorBuf.Length);
                resp.OutputStream.Close();
            }
            catch (Exception ex)
            {
                resp.StatusCode = 500;
                byte[] buf = Encoding.UTF8.GetBytes($"Error: {ex.Message}");
                resp.OutputStream.Write(buf, 0, buf.Length);
                resp.OutputStream.Close();
            }
        }
    }
}