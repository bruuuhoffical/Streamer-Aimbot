using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MemoryAim2
{
    public class AobMem2
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, IntPtr nSize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, IntPtr nSize, IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);


        [DllImport("kernel32.dll")]
        public static extern int ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        public struct PatternData
        {
            public byte[] pattern { get; set; }

            public byte[] mask { get; set; }
        }

        public struct MemoryPage
        {
            public IntPtr Start;

            public int Size;

            public MemoryPage(IntPtr start, int size)
            {
                Start = start;
                Size = size;
            }
        }

        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;

            public IntPtr AllocationBase;

            public uint AllocationProtect;

            public UIntPtr RegionSize;

            public uint State;

            public uint Protect;

            public uint Type;
        }

        public bool isPrivate;

        public int processId;

        public IntPtr _processHandle;

        private bool _enableCheck = true;

        public const uint MEM_COMMIT = 4096u;

        public const uint MEM_PRIVATE = 131072u;

        public const uint PAGE_READWRITE = 4u;

        public bool SetProcess(string[] processNames)
        {
            processId = 0;
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                string processName = process.ProcessName;
                if (Array.Exists(processNames, (string name) => name.Equals(processName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    processId = process.Id;
                    break;
                }
            }
            if (processId <= 0)
            {
                return false;
            }
            _processHandle = OpenProcess(ProcessAccessFlags.AllAccess, bInheritHandle: false, processId);
            if (_processHandle == IntPtr.Zero)
            {
                return false;
            }
            return true;
        }
        public void CheckProcess()
        {
            if (!_enableCheck)
            {
                return;
            }
            foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
            {
                IntPtr intPtr = OpenThread(ThreadAccess.SUSPEND_RESUME, bInheritHandle: false, (uint)thread.Id);
                if (intPtr != IntPtr.Zero)
                {
                    int num = 0;
                    do
                    {
                        num = ResumeThread(intPtr);
                    }
                    while (num > 0);
                    CloseHandle(intPtr);
                }
            }
        }

        public async Task<IEnumerable<long>> AoBScan(string bytePattern)
        {
            return await AobScan(bytePattern);
        }

        private async Task<IEnumerable<long>> AobScan(string pattern)
        {
            PatternData patternData = GetPatternDataFromPattern(pattern);
            List<long> addressRet = new List<long>();
            await Task.Run(delegate
            {
                List<MemoryPage> list = new List<MemoryPage>();
                IntPtr intPtr = IntPtr.Zero;
                MEMORY_BASIC_INFORMATION lpBuffer;
                while (VirtualQueryEx(_processHandle, intPtr, out lpBuffer, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) == (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)))
                {
                    if (CanReadPage(lpBuffer))
                    {
                        list.Add(new MemoryPage(intPtr, (int)lpBuffer.RegionSize.ToUInt64()));
                    }
                    intPtr = (IntPtr)((long)lpBuffer.BaseAddress + (long)(ulong)lpBuffer.RegionSize);
                }
                int patternLength = patternData.pattern.Length;
                Parallel.ForEach(list, delegate (MemoryPage addresss)
                {
                    byte[] array = new byte[addresss.Size];
                    if (ReadProcessMemory(_processHandle, addresss.Start, array, (IntPtr)addresss.Size, out var lpNumberOfBytesRead))
                    {
                        int num = -patternLength;
                        do
                        {
                            num = FindPattern(array, patternData.pattern, patternData.mask, num + patternLength);
                            if (num >= 0)
                            {
                                lock (addressRet)
                                {
                                    addressRet.Add((long)addresss.Start + num);
                                }
                            }
                        }
                        while (num != -1);
                    }
                    Array.Resize(ref array, (int)lpNumberOfBytesRead);
                });
            });
            return addressRet.OrderBy((long c) => c).AsEnumerable();
        }

        public bool CanReadPage(MEMORY_BASIC_INFORMATION page)
        {
            if (page.State == 4096 && page.Type == 131072)
            {
                return page.Protect == 4;
            }
            return false;
        }

        private PatternData GetPatternDataFromPattern(string pattern)
        {
            string[] patternParts = pattern.Split(' ');

            PatternData patternData = new PatternData
            {
                pattern = patternParts.Select(s => s.Contains("??") ? (byte)0x00 : byte.Parse(s, NumberStyles.HexNumber)).ToArray(),
                mask = patternParts.Select(s => s.Contains("??") ? (byte)0x00 : (byte)0xFF).ToArray()
            };

            return patternData;
        }


        public bool AobReplace(long address, string bytePattern)
        {
            try
            {
                byte[] array = StringToByteArray(bytePattern);
                return WriteProcessMemory(_processHandle, (IntPtr)address, array, (IntPtr)array.Length, IntPtr.Zero);
            }
            catch (Exception)
            {
            }
            return false;
        }

        public bool AobReplace(long address, int bytePattern)
        {
            byte[] bytes = BitConverter.GetBytes(bytePattern);
            return WriteProcessMemory(_processHandle, (IntPtr)address, bytes, (IntPtr)bytes.Length, IntPtr.Zero);
        }

        public async Task<int> ReadIntAsync(long addressToRead)
        {
            return await Task.Run(() => ReadInt(addressToRead));
        }

        public int ReadInt(long addressToRead)
        {
            byte[] array = new byte[4];
            if (ReadProcessMemory(_processHandle, (IntPtr)addressToRead, array, (IntPtr)array.Length, out var _))
            {
                return BitConverter.ToInt32(array, 0);
            }
            return 0;
        }

        public float ReadFloat(long addressToRead)
        {
            byte[] array = new byte[4];
            if (ReadProcessMemory(_processHandle, (IntPtr)addressToRead, array, (IntPtr)array.Length, out var _))
            {
                return BitConverter.ToSingle(array, 0);
            }
            return 0f;
        }
        public byte ReadHexByte(long addressToRead)
        {
            byte[] array = new byte[1];
            if (ReadProcessMemory(_processHandle, (IntPtr)addressToRead, array, (IntPtr)array.Length, out var _))
            {
                return array[0];
            }
            return 0;
        }

        public short ReadInt16(long addressToRead)
        {
            byte[] array = new byte[2];
            if (ReadProcessMemory(_processHandle, (IntPtr)addressToRead, array, (IntPtr)array.Length, out var _))
            {
                return BitConverter.ToInt16(array, 0);
            }
            return 0;
        }

        public string ReadString(long addressToRead, int size)
        {
            byte[] buffer = new byte[size];
            IntPtr bytesRead;

            bool readSuccess = ReadProcessMemory(_processHandle, (IntPtr)addressToRead, buffer, (IntPtr)size, out bytesRead);

            if (readSuccess && bytesRead.ToInt64() == size)
            {
                return BitConverter.ToString(buffer).Replace("-", " ");
            }
            return "";
        }
        private byte[] StringToByteArray(string hexString)
        {
            return (from hex in hexString.Split(' ')
                    select byte.Parse(hex, NumberStyles.HexNumber)).ToArray();
        }

        private int FindPattern(byte[] body, byte[] pattern, byte[] masks, int start = 0)
        {
            int result = -1;
            if (body.Length == 0 || pattern.Length == 0 || start > body.Length - pattern.Length || pattern.Length > body.Length)
            {
                return result;
            }
            for (int i = start; i <= body.Length - pattern.Length; i++)
            {
                if ((body[i] & masks[0]) != (pattern[0] & masks[0]))
                {
                    continue;
                }
                bool flag = true;
                for (int num = pattern.Length - 1; num >= 1; num--)
                {
                    if ((body[i + num] & masks[num]) != (pattern[num] & masks[num]))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }
    }


    #region ProcessAccessFlags
    /// <summary>
    /// Process access rights list.
    /// </summary>
    [Flags]
    public enum ProcessAccessFlags
    {
        /// <summary>
        /// All possible access rights for a process object.
        /// </summary>
        AllAccess = 0x001F0FFF,
        /// <summary>
        /// Required to create a process.
        /// </summary>
        CreateProcess = 0x0080,
        /// <summary>
        /// Required to create a thread.
        /// </summary>
        CreateThread = 0x0002,
        /// <summary>
        /// Required to duplicate a handle using DuplicateHandle.
        /// </summary>
        DupHandle = 0x0040,
        /// <summary>
        /// Required to retrieve certain information about a process, such as its token, exit code, and priority class (see OpenProcessToken).
        /// </summary>
        QueryInformation = 0x0400,
        /// <summary>
        /// Required to retrieve certain information about a process (see GetExitCodeProcess, GetPriorityClass, IsProcessInJob, QueryFullProcessImageName). 
        /// A handle that has the PROCESS_QUERY_INFORMATION access right is automatically granted PROCESS_QUERY_LIMITED_INFORMATION.
        /// </summary>
        QueryLimitedInformation = 0x1000,
        /// <summary>
        /// Required to set certain information about a process, such as its priority class (see SetPriorityClass).
        /// </summary>
        SetInformation = 0x0200,
        /// <summary>
        /// Required to set memory limits using SetProcessWorkingSetSize.
        /// </summary>
        SetQuota = 0x0100,
        /// <summary>
        /// Required to suspend or resume a process.
        /// </summary>
        SuspendResume = 0x0800,
        /// <summary>
        /// Required to terminate a process using TerminateProcess.
        /// </summary>
        Terminate = 0x0001,
        /// <summary>
        /// Required to perform an operation on the address space of a process (see VirtualProtectEx and WriteProcessMemory).
        /// </summary>
        VmOperation = 0x0008,
        /// <summary>
        /// Required to read memory in a process using <see cref="ReadProcessMemory"/>.
        /// </summary>
        VmRead = 0x0010,
        /// <summary>
        /// Required to write to memory in a process using WriteProcessMemory.
        /// </summary>
        VmWrite = 0x0020,
        /// <summary>
        /// Required to wait for the process to terminate using the wait functions.
        /// </summary>
        Synchronize = 0x00100000
    }
    #endregion


    [Flags]
    public enum ThreadAccess : int
    {
        TERMINATE = (0x0001),
        SUSPEND_RESUME = (0x0002),
        GET_CONTEXT = (0x0008),
        SET_CONTEXT = (0x0010),
        SET_INFORMATION = (0x0020),
        QUERY_INFORMATION = (0x0040),
        SET_THREAD_TOKEN = (0x0080),
        IMPERSONATE = (0x0100),
        DIRECT_IMPERSONATION = (0x0200)
    }

    public enum AllocationProtectEnum : uint
    {
        PAGE_EXECUTE = 0x00000010,
        PAGE_EXECUTE_READ = 0x00000020,
        PAGE_EXECUTE_READWRITE = 0x00000040,
        PAGE_EXECUTE_WRITECOPY = 0x00000080,
        PAGE_NOACCESS = 0x00000001,
        PAGE_READONLY = 0x00000002,
        PAGE_READWRITE = 0x00000004,
        PAGE_WRITECOPY = 0x00000008,
        PAGE_GUARD = 0x00000100,
        PAGE_NOCACHE = 0x00000200,
        PAGE_WRITECOMBINE = 0x00000400
    }

    public enum StateEnum : uint
    {
        MEM_COMMIT = 0x1000,
        MEM_FREE = 0x10000,
        MEM_RESERVE = 0x2000
    }
    public enum TypeEnum : uint
    {
        MEM_IMAGE = 0x1000000,
        MEM_MAPPED = 0x40000,
        MEM_PRIVATE = 0x20000
    }
}