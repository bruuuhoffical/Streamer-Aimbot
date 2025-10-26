using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aimbot.Mem
{
    public static class GetProcess
    {
        private static readonly string[] TargetProcesses = new[]
        {
            "HD-Player",        // BlueStacks
            "NoxVMHandle",      // NoxPlayer
            "LdBoxHeadless",    // LDPlayer
            "MEmuHeadless",     // MEmu
            "AndroidEmulatorEn",// Gameloop
            "VBoxHeadless",     // Genymotion / Andy
            "AndyConsole",      // Andy
            "Droid4X",          // Droid4X
            "KoPlayer",         // KoPlayer
            "ProjectTitan"      // SmartGaGa
        };

        public static string GetRunningEmulatorProcessName()
        {
            foreach (var name in TargetProcesses)
            {
                if (Process.GetProcessesByName(name).Any())
                {
                    return name;
                }
            }
            //MessageBox.Show("No supported emulator process is running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        public static (string processName, int pid) GetRunningEmulatorProcess()
        {
            foreach (var name in TargetProcesses)
            {
                var processes = Process.GetProcessesByName(name);
                if (processes.Any())
                {
                    return (name, processes[0].Id);
                }
            }
            //MessageBox.Show("No supported emulator process is running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return (null, 0);
        }
    }
}