using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.MemoryReading
{
    class MemoryReader
    {
        private const int PROCESS_VM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        public IntPtr MainModuleAddr { get { return process == null ? IntPtr.Zero : process.MainModule.BaseAddress; } }
        public bool IsInitialized { get { return process != null && !process.HasExited; } }

        private Process process = null;
        private IntPtr processHandle;
        
        public bool AttachTo(string processName)
        {
            bool success = false;
            var processesFound = Process.GetProcessesByName(processName);
            if (processesFound.Length > 0)
            {
                process = processesFound[0];
                processHandle = OpenProcess(PROCESS_VM_READ, false, process.Id);
                if (processHandle != null)
                {
                    success = true;
                }
            }
            return success;
        }

        public byte[] ReadBytes(int address, int size)
        {
            int bytesRead = 0;
            byte[] result;
            byte[] buffer = new byte[size];
            ReadProcessMemory((int)processHandle, address, buffer, size, ref bytesRead);
            if(bytesRead == size)
            {
                return buffer;
            } else
            {
                result = new byte[bytesRead];
                Array.Copy(buffer, result, bytesRead);
                return result;
            }
        }
    }
}
