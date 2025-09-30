using Aimbot.Mem;
using hideit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryAim2;
namespace Aimbot
{
    public  class Cheat
    {
        AobMem2 memoryfast = new AobMem2();

        public static bool aimbotHead = false;
        public static bool aimbotDrag = false;
        public static bool aimbotDragPro = false;

        private static readonly string aimbotHeadAob = "FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 A5 43";
        private const long NeckOffsetH = 0x80;
        private const long ChestOffsetH = 0x7C;

        private static readonly string aimbotDragAob = "FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43";
        private const long NeckOffsetD = 0xD2;
        private const long ChestOffsetD = 0x9E;

        private static readonly string aimbotDragProAob = "FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 A5 43";
        private const long NeckOffsetDP = 0xAA;
        private const long ChestOffsetDP = 0xA6;

        public static List<string> scopeTRSearchList = new List<string>
        {
            "33 33 93 3F 8F C2 F5 3C CD CC CC 3D 02 00 00 00 EC 51 B8 3D CD CC 4C 3F 00 00 00 00 00 00 A0 42 00 00 C0 3F 33 33 13 40 00 00 F0 3F 00 00 80 3F 01 00",
            "33 33 93 3F 8F C2 F5 3C CD CC CC 3D 02 00 00 00 EC 51 B8 3D CD CC 4C 3F 00 00 00 00 00 00 A0 42 00 00 C0 3F 33 33 13 40 00 00 F0 3F 00 00 29 5C 01 00"
        };

        public static List<string> scopeTRReplaceList = new List<string>
        {
            "20 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D ?? 00 00 00 29 5C 8F 3D 00 00 00 3F 00 00 F0 41 00 00 48 42 00 00 00 3F 33 33 13 40 00 00 D0 3F 00 00 80 3F 01",
            "20 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D ?? 00 00 00 29 5C 8F 3D 00 00 00 3F 00 00 F0 41 00 00 48 42 00 00 00 3F 33 33 13 40 00 00 D0 3F 00 00 80 5C 01"
        };

        private static readonly string search2x = "33 33 93 3f 8f c2 f5 3c cd cc cc 3d 02 00 00 00 ec 51 b8 3d cd cc 4c 3f 00 00 00 00 00 00 a0 42 00 00 c0 3f 33 33 13 40 00 00 f0 3f 00 00 80 3f 01 00";
        private static readonly string replace2x = "33 33 93 3f 8f c2 f5 3c cd cc cc 3d 02 00 00 00 ec 51 b8 3d cd cc 4c 3f 00 00 00 00 00 00 a0 42 00 00 c0 3f 33 33 13 40 00 00 f0 3f 00 00 29 5c 01 00";

        private static readonly string search4x = "20 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D ?? 00 00 00 29 5C 8F 3D 00 00 00 3F 00 00 F0 41 00 00 48 42 00 00 00 3F 33 33 13 40 00 00 D0 3F 00 00 80 3F 01";
        private static readonly string replace4x = "20 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D ?? 00 00 00 29 5C 8F 3D 00 00 00 3F 00 00 F0 41 00 00 48 42 00 00 00 3F 33 33 13 40 00 00 D0 3F 00 00 80 5C 01";
        
        private static readonly string searchRC = "03 0A 9F ED 10 0A 01 EE 00 0A 81 EE 10 0A 10 EE 10 8C BD E8 00 00 7A 44 F0";
        private static readonly string replaceRC = "03 0A 9F ED 10 0A 01 EE 00 0A 81 EE 10 0A 10 EE 10 8C BD E8 00 00 00 00 F0";
        
        private static readonly string sniperAimSearch = "01 00 00 00 00 00 00 00 00 00 00 00 41 00 00 00 00 00 00 00 01 00 00 00 CD CC";
        private static readonly string sniperAimReplace = "01 00 00 00 00 00 00 00 00 00 00 00 41 00 00 00 00 00 00 00 00 00 00 00 CD CC";
        
        private static readonly string sniperSwitchSearch = "3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F";
        private static readonly string sniperSwitchReplace = "01 00 00 80 00 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F";

        private static readonly string locationSAWMY = "20 00 00 00 69 00 6E 00 67 00 61 00 6D 00 65 00 2F 00 70 00 69 00 63 00 6B 00 75 00 70 00 2F 00 70 00 69 00 63 00 6B 00 75 00 70 00 5F 00 61 00 77 00 6D 00 5F 00 67 00 6F 00 6C 00 64 00";
        private static readonly string locationRAWMY = "1D 00 00 00 65 00 66 00 66 00 65 00 63 00 74 00 73 00 2F 00 76 00 66 00 78 00 5F 00 69 00 6E 00 61 00 67 00 6D 00 65 00 5F 00 6C 00 61 00 73 00 65 00 72 00 5F 00 73 00 68 00 6F 00 70 00";
        
        private static readonly string locationSM82B = "19 00 00 00 69 00 6E 00 67 00 61 00 6D 00 65 00 2F 00 70 00 69 00 63 00 6B 00 75 00 70 00 2F 00 70 00 69 00 63 00 6B 00 75 00 70 00 5F 00 62 00 6D 00 39 00 34 00 00 00 ?? ?? ?? ?? ?? ??";
        private static readonly string locationRM82B = "1D 00 00 00 65 00 66 00 66 00 65 00 63 00 74 00 73 00 2F 00 76 00 66 00 78 00 5F 00 69 00 6E 00 61 00 67 00 6D 00 65 00 5F 00 6C 00 61 00 73 00 65 00 72 00 5F 00 73 00 68 00 6F 00 70 00";

        private static readonly string locationSM24 = "18 00 00 00 69 00 6E 00 67 00 61 00 6D 00 65 00 2F 00 70 00 69 00 63 00 6B 00 75 00 70 00 2F 00 70 00 69 00 63 00 6B 00 75 00 70 00 5F 00 6D 00 32 00 34 ?? ?? ?? ?? ??";
        private static readonly string locationRM24 = "19 00 00 00 65 00 66 00 66 00 65 00 63 00 74 00 73 00 2F 00 76 00 66 00 78 00 5F 00 69 00 6E 00 67 00 61 00 6D 00 65 00 5F 00 6C 00 61 00 73 00 65 00 72 00 00 00 00 00";

        private static readonly string locationSVSK = "1A 00 00 00 69 00 6E 00 67 00 61 00 6D 00 65 00 2F 00 70 00 69 00 63 00 6B 00 75 00 70 00 2F 00 70 00 69 00 63 00 6B 00 75 00 70 00 5F 00 76 00 73 00 6B ?? ?? ?? ?? ??";
        private static readonly string locationRVSK = "19 00 00 00 65 00 66 00 66 00 65 00 63 00 74 00 73 00 2F 00 76 00 66 00 78 00 5F 00 69 00 6E 00 67 00 61 00 6D 00 65 00 5F 00 6C 00 61 00 73 00 65 00 72 00 00 00 00 00";

        private static readonly string aimfovSearch = "CD CC 4C 3E A4 70 FD 3E AE 47 01 3F A4 70 FD 3E AE 47 01 3F AE";
        private static readonly string aimfovReplace = "CD CC 4C 3E A4 70 FD 3E AE 47 E9 FF A4 70 FD 3E AE 47 01 3F AE";
        
        private static readonly string guestSearch = "";
        private static readonly string guestReplace = "";
        
        private static readonly string wallSearch = "";
        private static readonly string wallReplace = "";
        
        private static readonly string speedSearch = "";
        private static readonly string speedReplace = "";

        private static readonly BRCMem Memory = new BRCMem();

        #region Headshot

        #region Aimbot Head
        public static async Task<string> ActivateHeadAsync()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();
                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(aimbotHeadAob);
                if (matches == null || !matches.Any())
                {
                    return "No matches found for aimbot head pattern.";
                }

                foreach (var addr in matches)
                {
                    try
                    {
                        int neck = narzo.ReadInt(addr + NeckOffsetH);
                        int chest = narzo.ReadInt(addr + ChestOffsetH);

                        narzo.AobReplace(addr + ChestOffsetH, neck);
                        narzo.AobReplace(addr + NeckOffsetH, chest);
                    }
                    catch { }
                }

                aimbotHead = true;
                return "Aimbot Head Enabled!";
            }
            catch (Exception ex)
            {
                return $"Error enabling Aimbot Head: {ex.Message}";
            }
        }

        public static async Task<string> DisableHeadAsync()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();
                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(aimbotHeadAob);
                if (matches == null || !matches.Any())
                {
                    return "No matches found for aimbot head pattern.";
                }

                foreach (var addr in matches)
                {
                    try
                    {
                        int neck = narzo.ReadInt(addr + NeckOffsetH);
                        int chest = narzo.ReadInt(addr + ChestOffsetH);

                        narzo.AobReplace(addr + NeckOffsetH, chest);
                        narzo.AobReplace(addr + ChestOffsetH, neck);
                    }
                    catch { }
                }

                aimbotHead = false;
                return "Aimbot Head Disabled!";
            }
            catch (Exception ex)
            {
                return $"Error disabling Aimbot Head: {ex.Message}";
            }
        }

        #endregion

        #region Aimbot Drag
        public static async Task<string> ActivateDragAsync()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();
                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(aimbotDragAob);
                if (matches == null || !matches.Any())
                {
                    return "No matches found for aimbot drag pattern.";
                }

                foreach (var addr in matches)
                {
                    try
                    {
                        int neck = narzo.ReadInt(addr + NeckOffsetD);
                        int chest = narzo.ReadInt(addr + ChestOffsetD);

                        narzo.AobReplace(addr + ChestOffsetD, neck);
                        narzo.AobReplace(addr + NeckOffsetD, chest);
                    }
                    catch { }
                }

                aimbotDrag = true;
                return "Aimbot Drag Enabled!";
            }
            catch (Exception ex)
            {
                return $"Error enabling Aimbot Drag: {ex.Message}";
            }
        }

        public static async Task<string> DisableDragAsync()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();
                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(aimbotDragAob);
                if (matches == null || !matches.Any())
                {
                    return "No matches found for aimbot drag pattern.";
                }

                foreach (var addr in matches)
                {
                    try
                    {
                        int neck = narzo.ReadInt(addr + NeckOffsetD);
                        int chest = narzo.ReadInt(addr + ChestOffsetD);

                        narzo.AobReplace(addr + NeckOffsetD, chest);
                        narzo.AobReplace(addr + ChestOffsetD, neck);
                    }
                    catch { }
                }

                aimbotDrag = false;
                return "Aimbot Drag Disabled!";
            }
            catch (Exception ex)
            {
                return $"Error disabling Aimbot Drag: {ex.Message}";
            }
        }

        #endregion

        #region Aimbot Drag Pro
        public static async Task<string> ActivateDragProAsync()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();
                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(aimbotDragProAob);
                if (matches == null || !matches.Any())
                {
                    return "No matches found for aimbot drag pro pattern.";
                }

                foreach (var addr in matches)
                {
                    try
                    {
                        int neck = narzo.ReadInt(addr + NeckOffsetDP);
                        int chest = narzo.ReadInt(addr + ChestOffsetDP);

                        narzo.AobReplace(addr + ChestOffsetDP, neck);
                        narzo.AobReplace(addr + NeckOffsetDP, chest);
                    }
                    catch { }
                }

                aimbotDragPro = true;
                return "Aimbot Drag Pro Enabled!";
            }
            catch (Exception ex)
            {
                return $"Error enabling Aimbot Drag Pro: {ex.Message}";
            }
        }

        public static async Task<string> DisableDragProAsync()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();
                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(aimbotDragProAob);
                if (matches == null || !matches.Any())
                {
                    return "No matches found for aimbot Drag Pro pattern.";
                }

                foreach (var addr in matches)
                {
                    try
                    {
                        int neck = narzo.ReadInt(addr + NeckOffsetDP);
                        int chest = narzo.ReadInt(addr + ChestOffsetDP);

                        narzo.AobReplace(addr + NeckOffsetDP, chest);
                        narzo.AobReplace(addr + ChestOffsetDP, neck);
                    }
                    catch { }
                }

                aimbotDragPro = false;
                return "Aimbot Drag Pro Disabled!";
            }
            catch (Exception ex)
            {
                return $"Error disabling Aimbot DragPro: {ex.Message}";
            }
        }

        #endregion

        #region No Recoil
        public static async Task<string> EnableNoRecoil()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(searchRC);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, replaceRC);
                        if (result) replacedCount++;
                    }
                    catch
                    {
                       
                    }
                }
                return "No Recoil Enabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }

        public static async Task<string> DisableNoRecoil()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(replaceRC);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, searchRC);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "No Recoil Disabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }

        #endregion

        #region Scope Tracking
        public static async Task<string> EnableScopeTrackng2X()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(search2x);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, replace2x);
                        if (result) replacedCount++;
                    }
                    catch
                    {
                       
                    }
                }
                return "Scope Tracking 2x Enabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }

        public static async Task<string> DisableScopeTrackng2X()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(replace2x);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, search2x);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Scope Tracking 2x Disabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }

        public static async Task<string> EnableScopeTrackng4X()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(search4x);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, replace4x);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Scope Tracking 4x Enabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }

        public static async Task<string> DisableScopeTrackng4X()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(replace4x);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, search4x);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Scope Tracking 4x Disabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }

        #endregion

        #endregion

        #region Visuals

        #endregion

        #region Sniper

        #region Sniper Aim
        public static async Task<string> EnableSniperAim()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(sniperAimSearch);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, sniperAimReplace);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Sniper Aim Enabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }

        public static async Task<string> DisableSniperAim()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(sniperAimReplace);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, sniperAimSearch);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Sniper Aim Disabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }
        #endregion

        #region Sniper Switch
        public static async Task<string> EnableSniperSwitch()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(sniperSwitchSearch);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, sniperSwitchReplace);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Sniper Switch Enabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }

        public static async Task<string> DisableSniperSwitch()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(sniperSwitchReplace);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, sniperSwitchSearch);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Sniper Switch Disabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }
        #endregion

        #region Sniper Location AWMY
        public static async Task<string> EnableSniperLocationAWMY()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(locationSAWMY);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, locationRAWMY);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Location AWMY Enabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }

        public static async Task<string> DisableSniperLocationAWMY()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(locationRAWMY);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, locationSAWMY);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Location AWMY Disabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }
        #endregion

        #region Sniper Location M82B
        public static async Task<string> EnableSniperLocationM82B()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(locationSM82B);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, locationRM82B);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Location M82B Enabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }

        public static async Task<string> DisableSniperLocationM82B()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(locationRM82B);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, locationSM82B);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Location M82B Disabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }
        #endregion

        #region Sniper Location M24
        public static async Task<string> EnableSniperLocationM24()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(locationSM24);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, locationRM24);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Location M24 Enabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }

        public static async Task<string> DisableSniperLocationM24()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(locationRM24);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, locationSM24);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Location M24 Disabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }
        #endregion

        #region Sniper Location VSK
        public static async Task<string> EnableSniperLocationVSK()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(locationSVSK);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, locationRVSK);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Location VSK Enabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }

        public static async Task<string> DisableSniperLocationVSK()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(locationRVSK);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, locationSVSK);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Location VSK Disabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }
        #endregion

        #endregion

        #region Extras

        #region Aimfov90
        public static async Task<string> EnableAimfov90()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(aimfovSearch);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, aimfovReplace);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Aimfov 90 Enabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }

        public static async Task<string> DisableAimfov90()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                {
                    return "Failed to attach to emulator process.";
                }

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(aimfovReplace);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, aimfovSearch);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Aimfov 90 Disabled.";
            }
            catch (Exception ex)
            {
                return $"Error in test(): {ex.Message}";
            }
        }
        #endregion
        #endregion
    }
}

