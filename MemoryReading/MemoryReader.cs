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

        public IntPtr MainModuleAddr { get { return Process == null ? IntPtr.Zero : Process.MainModule.BaseAddress; } }
        public bool IsProcessOpened { get { return Process != null && !Process.HasExited; } }
        public Process Process { get; set; }
        private IntPtr ProcessHandle { get; set; }
        
        public bool AttachTo(string processName)
        {
            bool success = false;
            var processesFound = Process.GetProcessesByName(processName);
            if (processesFound.Length > 0)
            {
                Process = processesFound[0];
                ProcessHandle = OpenProcess(PROCESS_VM_READ, false, Process.Id);
                if (ProcessHandle != null)
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
            ReadProcessMemory((int)ProcessHandle, address, buffer, size, ref bytesRead);
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

        public string ReadString(int address, int maxLength)
        {
            var bytes = ReadBytes(address, maxLength);
            return Encoding.UTF8.GetString(bytes);
        }

        public int ReadInt(int address)//TODO crash on game being launched
        {
            return BitConverter.ToInt32(ReadBytes(address, 4), 0);
        }

        public bool ReadBoolean(int address)
        {
            return BitConverter.ToBoolean(ReadBytes(address, 1), 0);
        }

        public float ReadFloat(int address)
        {
            try
            {
                return BitConverter.ToSingle(ReadBytes(address, 4), 0);
            } catch
            {
                throw;
            }
        }
    }
}
