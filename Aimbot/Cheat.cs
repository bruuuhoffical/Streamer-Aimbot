using Aimbot.Mem;
using hideit;
using MemoryAim2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace Aimbot
{
    public  class Cheat
    {
        AobMem2 memoryfast = new AobMem2();

        public static bool aimbotHead = false;
        public static bool aimbotDrag = false;
        public static bool aimbotDragPro = false;

        private static readonly string aimbotHeadAob = "FF FF ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 ?? ?? ?? ?? ?? ?? ?? ?? 00 00 A5 43";
        private const long NeckOffsetH = 0x80;
        private const long ChestOffsetH = 0x7C;

        private static readonly string aimbotDragAob = "FF FF ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 ?? ?? ?? ?? ?? ?? ?? ?? 00 00 A5 43";
        private const long NeckOffsetD = 0xAA;
        private const long ChestOffsetD = 0xA6;

        private static readonly string aimbotDragProAob = "FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 A5 43";
        private const long NeckOffsetDP = 0x168;
        private const long ChestOffsetDP = 0x172;

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

        private static readonly string search4x = "00 00 00 00 FF FF FF FF 86 A3 03 00 FD 7E 03 00 00 7F 03 00 FD 7E 03 00 01 00 00 00 9A 99 99 3E F9 7E 03 00 04 00 00 00 00 00 20 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D ?? 00 00 00 29 5C 8F 3D 00 00 00 3F 00 00 F0 41 00 00 48 42 00 00 00 3F 33 33 13 40 00 00 D0 3F 00 00 80 3F 01";
        private static readonly string replace4x = "00 00 00 00 FF FF FF FF 86 A3 03 00 FD 7E 03 00 00 7F 03 00 FD 7E 03 00 01 00 00 00 9A 99 99 3E F9 7E 03 00 04 00 00 00 00 00 20 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D ?? 00 00 00 29 5C 8F 3D 00 00 00 3F 00 00 F0 41 00 00 48 42 00 00 00 3F 33 33 13 40 00 00 D0 3F 00 00 80 5C 01";
        
        private static readonly string searchRC = "30 48 2D E9 08 B0 8D E2 02 8B 2D ED 00 40 A0 E1 38 01 9F E5 00 00 8F E0 00 00 D0 E5";
        private static readonly string replaceRC = "00 00 A0 E3 1E FF 2F E1 02 8B 2D ED 00 40 A0 E1 38 01 9F E5 00 00 8F E0 00 00 D0 E5";
        
        private static readonly string searchF2M = "03 0A 9F ED 10 0A 01 EE 00 0A 81 EE 10 0A 10 EE 10 8C BD E8 00 00 7A 44 F0";
        private static readonly string replaceF2M = "03 0A 9F ED 10 0A 01 EE 00 0A 81 EE 10 0A 10 EE 10 8C BD E8 00 00 00 00 F0";
        
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
        
        private static readonly string guestSearch = "10 4C 2D E9 08 B0 8D E2 0C 01 9F E5 00 00 8F E0 00 00 D0 E5 00 00 50 E3 06 00 00 1A FC 00 9F E5 00 00 9F E7 00 00 90 E5 BE";
        private static readonly string guestReplace = "01 00 A0 E3 1E FF 2F E1 0C 01 9F E5 00 00 8F E0";
        
        private static readonly string wallSearch = "3F AE 47 81 3F AE 47 81 3F AE 47 81 3F 00";
        private static readonly string wallReplace = "3F AE 47 81 3F AE 47 81 BF AE 47 81 3F AE";
        private static List<ulong> WallAddress = new List<ulong>();

        private static readonly string speedSearch = "00 01 00 00 00 02 2B 07 3D";
        private static readonly string speedReplace = "00 01 00 00 00 92 E4 50 3D";
        private static List<ulong> SpeedAddress = new List<ulong>();

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
                return $"Error in No Recoil: {ex.Message}";
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
                return $"Error in No Recoil: {ex.Message}";
            }
        }

        #endregion

        #region Female to Male
        public static async Task<string> EnableF2M()
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

                var matches = await narzo.AoBScan(searchF2M);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, replaceF2M);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Female 2 Male Enabled.";
            }
            catch (Exception ex)
            {
                return $"Error in Female 2 Male: {ex.Message}";
            }
        }

        public static async Task<string> DisableF2M()
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

                var matches = await narzo.AoBScan(replaceF2M);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, searchF2M);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Female 2 Male Disabled.";
            }
            catch (Exception ex)
            {
                return $"Error in Female 2 Male: {ex.Message}";
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
                return $"Error in Scope Tracking 2x: {ex.Message}";
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
                return $"Error in Scope Tracking 2x: {ex.Message}";
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
                return $"Error in Scope Tracking 4x: {ex.Message}";
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
                return $"Error in Scope Tracking 4x: {ex.Message}";
            }
        }

        #endregion

        #endregion

        #region Visuals

        #region Chams Start
        public static async Task<string> ChamsStart()
        {
            string processName = "HD-Player";
            string resourceName = "Aimbot.Properties.glew32.dll";
            string glewpath = Path.Combine(Path.GetTempPath(), "glew32.dll");
            Cheat.inject(resourceName, glewpath);
            Process[] targetProcesses = Process.GetProcessesByName(processName);
            if (targetProcesses.Length == 0)
            {
                return "Emulator Not Running";
            }
            Process targetProcess = targetProcesses[0];
            IntPtr hProcess = Cheat.OpenProcess(1082U, false, targetProcess.Id);
            if (hProcess == IntPtr.Zero)
            {
                return "Chams Start Failed";
            }
            IntPtr loadLibraryAddr = Cheat.GetProcAddress(Cheat.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            IntPtr allocMemAddress = Cheat.VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)glewpath.Length, 4096U, 4U);
            IntPtr bytesWritten;
            Cheat.WriteProcessMemory(hProcess, allocMemAddress, Encoding.ASCII.GetBytes(glewpath), checked((uint)glewpath.Length), out bytesWritten);
            if (Cheat.CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0U, IntPtr.Zero) == IntPtr.Zero)
            {
                return "Chams Start Failed";
            }
            else
            {
                return "Chams Started Injected";
            }
        }
        #endregion

        #region Chams Menu Old
        public static async Task<string> ChamsMenuOld()
        {
            string processName = "HD-Player";
            string resourceName = "Aimbot.Properties.cnormal.dll";
            string glewpath = Path.Combine(Path.GetTempPath(), "cnormal.dll");
            Cheat.inject(resourceName, glewpath);
            Process[] targetProcesses = Process.GetProcessesByName(processName);
            if (targetProcesses.Length == 0)
            {
                return "Emulator Not Running";
            }
            Process targetProcess = targetProcesses[0];
            IntPtr hProcess = Cheat.OpenProcess(1082U, false, targetProcess.Id);
            if (hProcess == IntPtr.Zero)
            {
                return "Chams Menu Old Failed";
            }
            IntPtr loadLibraryAddr = Cheat.GetProcAddress(Cheat.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            IntPtr allocMemAddress = Cheat.VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)glewpath.Length, 4096U, 4U);
            IntPtr bytesWritten;
            Cheat.WriteProcessMemory(hProcess, allocMemAddress, Encoding.ASCII.GetBytes(glewpath), checked((uint)glewpath.Length), out bytesWritten);
            if (Cheat.CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0U, IntPtr.Zero) == IntPtr.Zero)
            {
                return "Chams Menu Old Failed";
            }
            else
            {
                return "Chams Menu Old Injected";
            }
        }
        #endregion

        #region Chams Menu New
        public static async Task<string> ChamsMenuNew()
        {
            string processName = "HD-Player";
            string resourceName = "Aimbot.Properties.coverlay.dll";
            string glewpath = Path.Combine(Path.GetTempPath(), "coverlay.dll");
            Cheat.inject(resourceName, glewpath);
            Process[] targetProcesses = Process.GetProcessesByName(processName);
            if (targetProcesses.Length == 0)
            {
                return "Emulator Not Running";
            }
            Process targetProcess = targetProcesses[0];
            IntPtr hProcess = Cheat.OpenProcess(1082U, false, targetProcess.Id);
            if (hProcess == IntPtr.Zero)
            {
                return "Chams Menu New Failed";
            }
            IntPtr loadLibraryAddr = Cheat.GetProcAddress(Cheat.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            IntPtr allocMemAddress = Cheat.VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)glewpath.Length, 4096U, 4U);
            IntPtr bytesWritten;
            Cheat.WriteProcessMemory(hProcess, allocMemAddress, Encoding.ASCII.GetBytes(glewpath), checked((uint)glewpath.Length), out bytesWritten);
            if (Cheat.CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0U, IntPtr.Zero) == IntPtr.Zero)
            {
                return "Chams Menu New Failed";
            }
            else
            {
                return "Chams Menu New Injected";
            }
        }
        #endregion

        #region Chams 3D
        public static async Task<string> Chams3D()
        {
            string processName = "HD-Player";
            string resourceName = "Aimbot.Properties.transparent.dll";
            string glewpath = Path.Combine(Path.GetTempPath(), "transparent.dll");
            Cheat.inject(resourceName, glewpath);
            Process[] targetProcesses = Process.GetProcessesByName(processName);
            if (targetProcesses.Length == 0)
            {
                return "Emulator Not Running";
            }
            Process targetProcess = targetProcesses[0];
            IntPtr hProcess = Cheat.OpenProcess(1082U, false, targetProcess.Id);
            if (hProcess == IntPtr.Zero)
            {
                return "Chams 3D Failed";
            }
            IntPtr loadLibraryAddr = Cheat.GetProcAddress(Cheat.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            IntPtr allocMemAddress = Cheat.VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)glewpath.Length, 4096U, 4U);
            IntPtr bytesWritten;
            Cheat.WriteProcessMemory(hProcess, allocMemAddress, Encoding.ASCII.GetBytes(glewpath), checked((uint)glewpath.Length), out bytesWritten);
            if (Cheat.CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0U, IntPtr.Zero) == IntPtr.Zero)
            {
                return "Chams 3D Failed";
            }
            else
            {
                return "Chams 3D Injected";
            }
        }
        #endregion

        #region Chams HDR MAP
        public static async Task<string> ChamsHDR()
        {
            string processName = "HD-Player";
            string resourceName = "Aimbot.Properties.MapHdr.dll";
            string glewpath = Path.Combine(Path.GetTempPath(), "MapHdr.dll");
            Cheat.inject(resourceName, glewpath);
            Process[] targetProcesses = Process.GetProcessesByName(processName);
            if (targetProcesses.Length == 0)
            {
                return "Emulator Not Running";
            }
            Process targetProcess = targetProcesses[0];
            IntPtr hProcess = Cheat.OpenProcess(1082U, false, targetProcess.Id);
            if (hProcess == IntPtr.Zero)
            {
                return "Chams HDR Failed";
            }
            IntPtr loadLibraryAddr = Cheat.GetProcAddress(Cheat.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            IntPtr allocMemAddress = Cheat.VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)glewpath.Length, 4096U, 4U);
            IntPtr bytesWritten;
            Cheat.WriteProcessMemory(hProcess, allocMemAddress, Encoding.ASCII.GetBytes(glewpath), checked((uint)glewpath.Length), out bytesWritten);
            if (Cheat.CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0U, IntPtr.Zero) == IntPtr.Zero)
            {
                return "Chams HDR Failed";
            }
            else
            {
                return "Chams HDR Injected";
            }
        }
        #endregion

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
                return $"Error in Sniper Aim: {ex.Message}";
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
                return $"Error in Sniper Aim: {ex.Message}";
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
                return $"Error in Sniper Switch: {ex.Message}";
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
                return $"Error in Sniper Switch: {ex.Message}";
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
                return $"Error in Location AWMY: {ex.Message}";
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
                return $"Error in Location AWMY: {ex.Message}";
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
                return $"Error in Location M82B: {ex.Message}";
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
                return $"Error in Location M82B: {ex.Message}";
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
                return $"Error in Location M24: {ex.Message}";
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
                return $"Error in Location M24: {ex.Message}";
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
                return $"Error in Location VSK: {ex.Message}";
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
                return $"Error in Location VSK: {ex.Message}";
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
                return $"Error in Aimfov 90: {ex.Message}";
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
                return $"Error in Aimfov 90: {ex.Message}";
            }
        }
        #endregion

        #region WallHack

        public static async Task<string> LoadWall()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                    return "Failed to attach to emulator process.";

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(wallSearch);
                if (matches == null || !matches.Any())
                    return "No Pattern Found";

                WallAddress = matches.Select(addr => (ulong)addr).ToList();
                return $"Wall Hack Loaded";
            }
            catch (Exception ex)
            {
                return $"Error in Load Wall: {ex.Message}";
            }
        }

        public static async Task<string> EnableWall()
        {
            try
            {
                if (WallAddress == null || WallAddress.Count == 0)
                    return "No loaded addresses";

                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                    return "Failed to attach to emulator process.";

                narzo.CheckProcess();

                int replacedCount = 0;
                foreach (var addr in WallAddress)
                {
                    try
                    {
                        if (narzo.AobReplace((long)addr, wallReplace))
                            replacedCount++;
                    }
                    catch { }
                }

                return $"Wall Hack Enabled";
            }
            catch (Exception ex)
            {
                return $"Error in Enable Wall: {ex.Message}";
            }
        }

        public static async Task<string> DisableWall()
        {
            try
            {
                if (WallAddress == null || WallAddress.Count == 0)
                    return "No loaded addresses";

                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                    return "Failed to attach to emulator process.";

                narzo.CheckProcess();

                int replacedCount = 0;
                foreach (var addr in WallAddress)
                {
                    try
                    {
                        if (narzo.AobReplace((long)addr, wallSearch))
                            replacedCount++;
                    }
                    catch { }
                }

                return $"Wall Hack Disabled";
            }
            catch (Exception ex)
            {
                return $"Error in Disable Wall: {ex.Message}";
            }
        }

        #endregion

        #region SpeedHack

        public static async Task<string> LoadSpeed()
        {
            try
            {
                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                    return "Failed to attach to emulator process.";

                narzo.CheckProcess();

                var matches = await narzo.AoBScan(speedSearch);
                if (matches == null || !matches.Any())
                    return "No Pattern Found";

                SpeedAddress = matches.Select(addr => (ulong)addr).ToList();
                return $"Speed Hack Loaded";
            }
            catch (Exception ex)
            {
                return $"Error in Load Speed: {ex.Message}";
            }
        }

        public static async  Task<string> EnableSpeed()
        {
            try
            {
                if (SpeedAddress == null || SpeedAddress.Count == 0)
                    return "No loaded addresses";

                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                    return "Failed to attach to emulator process.";

                narzo.CheckProcess();

                int replacedCount = 0;
                foreach (var addr in SpeedAddress)
                {
                    try
                    {
                        if (narzo.AobReplace((long)addr, speedReplace))
                            replacedCount++;
                    }
                    catch { }
                }

                return $"Speed Hack Enabled";
            }
            catch (Exception ex)
            {
                return $"Error in Enable Wall: {ex.Message}";
            }
        }

        public static async  Task<string> DisableSpeed()
        {
            try
            {
                if (SpeedAddress == null || SpeedAddress.Count == 0)
                    return "No loaded addresses";

                var emulator = GetProcess.GetRunningEmulatorProcessName();
                var narzo = new BRCMem();

                if (!narzo.SetProcess(new string[] { emulator }))
                    return "Failed to attach to emulator process.";

                narzo.CheckProcess();

                int replacedCount = 0;
                foreach (var addr in SpeedAddress)
                {
                    try
                    {
                        if (narzo.AobReplace((long)addr, speedSearch))
                            replacedCount++;
                    }
                    catch { }
                }

                return $"Speed Hack Disabled";
            }
            catch (Exception ex)
            {
                return $"Error in Disable Speed: {ex.Message}";
            }
        }

        #endregion

        #region Reset Guest
        public static async Task<string> ResetGuest()
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

                var matches = await narzo.AoBScan(guestSearch);
                if (matches == null || !matches.Any())
                {
                    return "No Pattern Found";
                }

                int replacedCount = 0;
                foreach (var addr in matches)
                {
                    try
                    {
                        bool result = narzo.AobReplace(addr, guestReplace);
                        if (result) replacedCount++;
                    }
                    catch
                    {

                    }
                }
                return "Guest Reset Success";
            }
            catch (Exception ex)
            {
                return $"Error in Guest: {ex.Message}";
            }
        }
        #endregion

        #endregion

        #region DLL INJECTION SYSTEM
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr LoadLibraryA(string lpLibFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeLibrary(IntPtr hModule);

        const uint PROCESS_CREATE_THREAD = 0x2;
        const uint PROCESS_QUERY_INFORMATION = 0x400;
        const uint PROCESS_VM_OPERATION = 0x8;
        const uint PROCESS_VM_WRITE = 0x20;
        const uint PROCESS_VM_READ = 0x10;
        const uint MEM_COMMIT = 0x1000;
        const uint PAGE_READWRITE = 4;

        private static void inject(string resourceName, string outputPath)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            using (Stream resourceStream = executingAssembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new ArgumentException($"Resource '{resourceName}' not found.");
                }
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create))
                {
                    byte[] buffer = new byte[resourceStream.Length];
                    resourceStream.Read(buffer, 0, buffer.Length);
                    fileStream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        #endregion
    }
}

