using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Aimbot.Mem;

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

        static Dictionary<int, string> HotkeyBinds = new Dictionary<int, string>();
        static Dictionary<int, string> MouseBinds = new Dictionary<int, string>();
        static object _hotkeyLock = new object();

        static int appPort = 3001;

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
            var (username, password) = LoadCredentials();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                try
                {
                    KeyAuthApp.login(username, password);
                    if (KeyAuthApp.response.success)
                    {
                        Console.WriteLine(" [+] Auto-login success!");
                        CurrentUserId = KeyAuthApp.user_data.username;
                        CurrentUsername = username;
                        LastAuth = DateTime.UtcNow;
                        StartHttpServer();
                        var handle = Program.GetConsoleWindow();
                        if (handle != IntPtr.Zero)
                        {
                            Program.ShowWindow(handle, 0);
                        }
                        ClearCommandHistory();
                        return; 
                    }
                    else
                    {
                        Console.WriteLine($" [+] Auto-login failed: {KeyAuthApp.response.message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" [+] Auto-login error: {ex.Message}");
                }
            }

            while (true)
            {
                Console.Write(" [+] Enter Username: ");
                username = Console.ReadLine();
                Console.Write(" [+] Enter Password: ");
                password = Console.ReadLine();

                try
                {
                    KeyAuthApp.login(username, password);
                    if (KeyAuthApp.response.success)
                    {
                        Console.WriteLine(" [+] Login success!");
                        CurrentUserId = KeyAuthApp.user_data.username;
                        CurrentUsername = username;
                        LastAuth = DateTime.UtcNow;
                        SaveCredentials(username, password); 
                        StartHttpServer();
                        var handle = Program.GetConsoleWindow();
                        if (handle != IntPtr.Zero)
                        {
                            Program.ShowWindow(handle, 0);
                        }
                        ClearCommandHistory();
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

        static void SaveCredentials(string username, string password)
        {
            try
            {
                string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.dat");
                File.WriteAllText(configPath, $"{username}\n{password}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [+] Error saving credentials: {ex.Message}");
            }
        }

        static (string username, string password) LoadCredentials()
        {
            try
            {
                string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.dat");
                if (File.Exists(configPath))
                {
                    string[] lines = File.ReadAllLines(configPath);
                    if (lines.Length >= 2)
                    {
                        return (lines[0], lines[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [+] Error loading credentials: {ex.Message}");
            }
            return (null, null);
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
            int port = appPort;
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

        static async void HandleHttpRequest(HttpListenerContext ctx)
        {
            var req = ctx.Request;
            var resp = ctx.Response;

            try
            {
                resp.AddHeader("Access-Control-Allow-Origin", "*");
                resp.AddHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                resp.AddHeader("Access-Control-Allow-Headers", "Content-Type");

                byte[] buf = null; 

                if (req.HttpMethod == "OPTIONS")
                {
                    resp.StatusCode = 204;
                    resp.Close();
                    return;
                }

                string path = req.Url.AbsolutePath.TrimStart('/').ToLower();

                if (req.HttpMethod == "GET")
                {
                    if (path == "status")
                    {
                        var (processName, pid) = GetProcess.GetRunningEmulatorProcess();
                        var respObj = new
                        {
                            online = processName != null,
                            connectedTo = processName ?? "None",
                            pid = pid
                        };
                        buf = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(respObj));
                        resp.ContentType = "application/json";
                        resp.ContentLength64 = buf.Length;
                        resp.OutputStream.Write(buf, 0, buf.Length);
                        resp.OutputStream.Close();
                        return;
                    }

                    string resName = null;
                    if (path == "" || path == "main" || path == "home")
                        resName = "Aimbot.Properties.main.html";

                    if (!string.IsNullOrEmpty(resName))
                    {
                        string html = ReadEmbedded(resName);
                        if (html == null)
                        {
                            resp.StatusCode = 404;
                            buf = Encoding.UTF8.GetBytes("404 Not Found");
                        }
                        else
                        {
                            buf = Encoding.UTF8.GetBytes(html);
                            resp.ContentType = "text/html";
                            resp.ContentLength64 = buf.Length;
                        }
                        resp.OutputStream.Write(buf, 0, buf.Length);
                    }
                    else
                    {
                        resp.StatusCode = 404;
                        buf = Encoding.UTF8.GetBytes("404 Not Found");
                        resp.OutputStream.Write(buf, 0, buf.Length);
                    }
                    resp.OutputStream.Close();
                    return;
                }

                if (req.HttpMethod == "POST" && path == "session")
                {
                    bool active = CurrentUserId != null && (DateTime.UtcNow - LastAuth) < SessionTimeout;
                    var respObj = new { active, username = CurrentUserId };
                    buf = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(respObj));
                    resp.ContentType = "application/json";
                    resp.OutputStream.Write(buf, 0, buf.Length);
                    resp.OutputStream.Close();
                    return;
                }
                if (req.HttpMethod == "POST" && (path == "" || path == "/"))
                {
                    string cmd = new StreamReader(req.InputStream).ReadToEnd().Trim();
                    string feedback = "OK";
                    bool isError = false;

                    try
                    {
                        switch (cmd.ToLower())
                        {
                            case "aimbotheadon":
                                feedback = await Cheat.ActivateHeadAsync();
                                break;
                            case "aimbotheadoff":
                                feedback = await Cheat.DisableHeadAsync();
                                break;
                            case "aimbotdragon":
                                feedback = await Cheat.ActivateDragAsync();
                                break;
                            case "aimbotdragoff":
                                feedback = await Cheat.DisableDragAsync();
                                break;
                            case "aimbotdragproon":
                                feedback = await Cheat.ActivateDragProAsync();
                                break;
                            case "aimbotdragprooff":
                                feedback = await Cheat.DisableDragProAsync();
                                break;
                            case "norecoilon":
                                feedback = await Cheat.EnableNoRecoil();
                                break;
                            case "norecoiloff":
                                feedback = await Cheat.DisableNoRecoil();
                                break;
                            case "scopetracking2x":
                                feedback = await Cheat.EnableScopeTrackng2X();
                                break;
                            case "scopetrackingoff2x":
                                feedback = await Cheat.DisableScopeTrackng2X();
                                break;
                            case "scopetracking4x":
                                feedback = await Cheat.EnableScopeTrackng4X();
                                break;
                            case "scopetrackingoff4x":
                                feedback = await Cheat.DisableScopeTrackng4X();
                                break;
                            //Visuals

                            //Sniper
                            case "sniperaimon":
                                feedback = await Cheat.EnableSniperAim();
                                break;
                            case "sniperaimoff":
                                feedback = await Cheat.DisableSniperAim();
                                break;
                            
                            case "sniperswitchon":
                                feedback = await Cheat.EnableSniperSwitch();
                                break;
                            case "sniperswitchoff":
                                feedback = await Cheat.DisableSniperSwitch();
                                break;
                            
                            case "awmylocationon":
                                feedback = await Cheat.EnableSniperLocationAWMY();
                                break;
                            case "awmylocationoff":
                                feedback = await Cheat.DisableSniperLocationAWMY();
                                break;
                            
                            case "m82blocationon":
                                feedback = await Cheat.EnableSniperLocationM82B();
                                break;
                            case "m82blocationoff":
                                feedback = await Cheat.DisableSniperLocationM82B();
                                break;

                            case "m24locationon":
                                feedback = await Cheat.EnableSniperLocationM24();
                                break;
                            case "m24locationoff":
                                feedback = await Cheat.DisableSniperLocationM24();
                                break;

                            case "vsklocationon":
                                feedback = await Cheat.EnableSniperLocationVSK();
                                break;
                            case "vsklocationoff":
                                feedback = await Cheat.DisableSniperLocationVSK();
                                break;
                            
                            //Extras
                            case "aimfovon":
                                feedback = await Cheat.EnableAimfov90();
                                break;
                            case "aimfovoff":
                                feedback = await Cheat.DisableAimfov90();
                                break;





                            case "exit":
                                Environment.Exit(0);
                                Application.Exit();
                                break;
                            case "restart":
                                Application.Restart();
                                break;
                            case "getCurrentStatus":
                                //var (processName, pid) = GetProcess.GetRunningEmulatorProcess();
                                //var respObj = new
                                //{
                                //    online = processName != null,
                                //    connectedTo = processName ?? "None",
                                //    pid = pid
                                //};
                                //buf = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(respObj));
                                //resp.ContentType = "application/json";
                                //resp.ContentLength64 = buf.Length;
                                //resp.OutputStream.Write(buf, 0, buf.Length);
                                //resp.OutputStream.Close();
                                return;
                            case "getStatus":
                                var (processName, pid) = GetProcess.GetRunningEmulatorProcess();
                                var respObj = new
                                {
                                    online = processName != null,
                                    connectedTo = processName ?? "None",
                                    pid = pid
                                };
                                buf = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(respObj));
                                resp.ContentType = "application/json";
                                resp.ContentLength64 = buf.Length;
                                resp.OutputStream.Write(buf, 0, buf.Length);
                                resp.OutputStream.Close();
                                return;
                            default:
                                if (cmd.StartsWith("sethotkey:"))
                                {
                                    var parts = cmd.Split(':');
                                    if (parts.Length == 3 && int.TryParse(parts[1], out int idx))
                                    {
                                        lock (_hotkeyLock)
                                        {
                                            if (HotkeyBinds.TryGetValue(idx, out string oldKey)) HotkeyBinds.Remove(idx);
                                            foreach (var k in HotkeyBinds.Where(kv => kv.Value.Equals(parts[2], StringComparison.OrdinalIgnoreCase)).ToList())
                                                HotkeyBinds.Remove(k.Key);
                                            HotkeyBinds[idx] = parts[2].ToUpper();
                                        }
                                        feedback = $"Hotkey for function {idx} set to {parts[2].ToUpper()}";
                                    }
                                    else
                                    {
                                        feedback = "Invalid setHotkey command";
                                        isError = true;
                                    }
                                }
                                else if (cmd.StartsWith("clearhotkey:"))
                                {
                                    var parts = cmd.Split(':');
                                    if (parts.Length == 2 && int.TryParse(parts[1], out int idx))
                                    {
                                        lock (_hotkeyLock)
                                        {
                                            if (HotkeyBinds.ContainsKey(idx))
                                            {
                                                HotkeyBinds.Remove(idx);
                                                feedback = $"Hotkey for function {idx} cleared";
                                            }
                                            else
                                                feedback = $"No hotkey set for function {idx}";
                                        }
                                    }
                                    else
                                    {
                                        feedback = "Invalid clearHotkey command";
                                        isError = true;
                                    }
                                }
                                else if (cmd.StartsWith("setmouse:"))
                                {
                                    var parts = cmd.Split(':');
                                    if (parts.Length == 3 && int.TryParse(parts[1], out int idx))
                                    {
                                        lock (_hotkeyLock)
                                        {
                                            if (MouseBinds.TryGetValue(idx, out string oldBtn)) MouseBinds.Remove(idx);
                                            foreach (var k in MouseBinds.Where(kv => kv.Value.Equals(parts[2], StringComparison.OrdinalIgnoreCase)).ToList())
                                                MouseBinds.Remove(k.Key);
                                            MouseBinds[idx] = parts[2].ToUpper();
                                        }
                                        feedback = $"Mouse button for function {idx} set to {parts[2].ToUpper()}";
                                    }
                                    else
                                    {
                                        feedback = "Invalid setMouse command";
                                        isError = true;
                                    }
                                }
                                else if (cmd.StartsWith("clearmouse:"))
                                {
                                    var parts = cmd.Split(':');
                                    if (parts.Length == 2 && int.TryParse(parts[1], out int idx))
                                    {
                                        lock (_hotkeyLock)
                                        {
                                            if (MouseBinds.ContainsKey(idx))
                                            {
                                                MouseBinds.Remove(idx);
                                                feedback = $"Mouse bind for function {idx} cleared";
                                            }
                                            else
                                                feedback = $"No mouse bind set for function {idx}";
                                        }
                                    }
                                    else
                                    {
                                        feedback = "Invalid clearMouse command";
                                        isError = true;
                                    }
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        feedback = "Error: " + ex.Message;
                        isError = true;
                    }

                    resp.ContentType = "text/plain";
                    resp.StatusCode = isError ? 400 : 200;
                    buf = Encoding.UTF8.GetBytes(feedback);
                    resp.OutputStream.Write(buf, 0, buf.Length);
                    resp.OutputStream.Close();
                    return;
                }

                resp.StatusCode = 404;
                buf = Encoding.UTF8.GetBytes("404 Not Found");
                resp.OutputStream.Write(buf, 0, buf.Length);
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

        static void OnHotkey(int idx)
        {
            if (idx == 2)
            {
                System.Windows.Forms.Application.DoEvents();
                MessageBox.Show("2");
            }
            else if (idx == 3)
            {
                System.Windows.Forms.Application.DoEvents();
                MessageBox.Show("3");
            }
        }

        static IntPtr _kbdHook = IntPtr.Zero;
        static LowLevelKeyboardProc _kbdProc = KbdHookCallback;
        delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        static IntPtr _mouseHook = IntPtr.Zero;
        static LowLevelMouseProc _mouseProc = MouseHookCallback;
        delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x0100;
        const int WH_MOUSE_LL = 14;
        const int WM_XBUTTONDOWN = 0x020B;

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", EntryPoint = "SetWindowsHookExW", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SetWindowsHookExMouse(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        public static void StartHooks()
        {
            if (_kbdHook == IntPtr.Zero)
            {
                _kbdHook = SetWindowsHookEx(WH_KEYBOARD_LL, _kbdProc, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
                MessageBox.Show($"[+] Keyboard hook installed: {_kbdHook != IntPtr.Zero}");
            }
            if (_mouseHook == IntPtr.Zero)
            {
                _mouseHook = SetWindowsHookExMouse(WH_MOUSE_LL, _mouseProc, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
                MessageBox.Show($"[+] Mouse hook installed: {_mouseHook != IntPtr.Zero}");
            }
        }

        public static void StopHooks()
        {
            if (_kbdHook != IntPtr.Zero) { UnhookWindowsHookEx(_kbdHook); _kbdHook = IntPtr.Zero; }
            if (_mouseHook != IntPtr.Zero) { UnhookWindowsHookEx(_mouseHook); _mouseHook = IntPtr.Zero; }
        }

        static IntPtr KbdHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                string key = VKToKeyString(vkCode);
                MessageBox.Show($"[+] Key pressed: {key} (VK: 0x{vkCode:X2})");
                lock (_hotkeyLock)
                {
                    foreach (var kv in HotkeyBinds)
                    {
                        MessageBox.Show($"[+] Checking hotkey: ID={kv.Key}, Key={kv.Value}");
                        if (key.Equals(kv.Value, StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show($"[+] Hotkey match for ID={kv.Key}");
                            OnHotkey(kv.Key);
                        }
                    }
                }
            }
            return CallNextHookEx(_kbdHook, nCode, wParam, lParam);
        }

        static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_XBUTTONDOWN)
            {
                int mouseButton = Marshal.ReadInt32(lParam + 8);
                string btn = mouseButton == 0x1 ? "X1" : mouseButton == 0x2 ? "X2" : "?";
                lock (_hotkeyLock)
                {
                    foreach (var kv in MouseBinds)
                    {
                        if (btn.Equals(kv.Value, StringComparison.OrdinalIgnoreCase))
                        {
                            OnHotkey(kv.Key);
                        }
                    }
                }
            }
            return CallNextHookEx(_mouseHook, nCode, wParam, lParam);
        }

        static string VKToKeyString(int vk)
        {
            if (vk >= 0x70 && vk <= 0x7B)
                return "F" + (vk - 0x6F);
            if (vk >= 0x30 && vk <= 0x39)
                return ((char)vk).ToString();
            if (vk >= 0x41 && vk <= 0x5A)
                return ((char)vk).ToString();
            if (vk == 0x1B) return "ESC";
            if (vk == 0x20) return "SPACE";
            return vk.ToString("X2");
        }
    }
}