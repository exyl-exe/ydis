using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ydis.Model.MemoryReading
{
    /// <summary>
    /// Class <c>MemoryReader</c> is a simple class to read values in the
    /// memory of a process without calling dll functions directly
    /// </summary>
    public class MemoryReader
    {
        /// <summary>
        /// Base address of the main module of the read process
        /// </summary>
        public IntPtr MainModuleAddr {
            get
            {
                if(Process != null)
                {
                    if (!_mainModuleLoaded)
                    {
                        var mainModule = Process.MainModule;
                        if (mainModule != null)
                        {
                            _mainModuleLoaded = true;
                            _mainModuleAddress = mainModule.BaseAddress;
                            mainModule.Disposed += ResetBaseAddress;
                        } else
                        {
                            _mainModuleAddress = IntPtr.Zero;
                        }
                    }
                    return _mainModuleAddress;
                } else
                {
                    return IntPtr.Zero;
                }             
            }
        }

        /// <summary>
        /// Boolean true if the read process is opened, false if it has terminated
        /// </summary>
        public bool IsProcessOpened { get { return Process != null && !Process.HasExited; } }

        /// <summary>
        /// Current process the reader is attached to
        /// </summary>
        public Process Process { get; set; }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        // flags to open a process with read perms 
        private const int PROCESS_VM_READ = 0x0010;

        // Handle for the current process
        private IntPtr ProcessHandle { get; set; }
        // MainModuleAddress is stored for performance (avoiding sys calls)
        private IntPtr _mainModuleAddress = IntPtr.Zero;
        // Wether the main module is loaded or not
        private bool _mainModuleLoaded = false;

        /// <summary>
        /// Attaches itself to a process with the given name
        /// </summary>
        /// <param name="processName">The name of the process which memory's will be read</param>
        /// <returns>true if the process was opened successfully, false otherwise</returns>
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

        /// <summary>
        /// Reads an array of bytes in the memory of the current process
        /// </summary>
        /// <param name="address">Base address of the byte array to read</param>
        /// <param name="size">Number of bytes to be read</param>
        /// <returns>An array of bytes read in the memory.
        /// Can be shorter than the given size if an error occured</returns>
        public byte[] ReadBytes(int address, int size)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[size];
            ReadProcessMemory((int)ProcessHandle, address, buffer, size, ref bytesRead);
            if (bytesRead == size)
            {
                return buffer;
            }
            else
            {
                throw new MemoryReadingException(size, bytesRead);
            }
        }

        /// <summary>
        /// Reads a string in the memory of the current process
        /// </summary>
        /// <param name="address">Address the string starts at in memory</param>
        /// <param name="maxLength">Length of the string to read</param>
        /// <returns>The string read at the give address in the memory.
        /// Can be shorted than the given size if an error occured.</returns>
        public string ReadString(int address, int maxLength)
        {
            var bytes = ReadBytes(address, maxLength);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Reads an integer in the memory of the current process
        /// </summary>
        /// <param name="address">The address where to read the integer</param>
        /// <returns>The integer at the given address in the memory.</returns>
        public int ReadInt(int address)
        {
            return BitConverter.ToInt32(ReadBytes(address, 4), 0);
        }

        /// <summary>
        /// Reads a boolean in the memory of the current process
        /// </summary>
        /// <param name="address">The address where to read the boolean</param>
        /// <returns>The boolean at the given address in the memory</returns>
        public bool ReadBoolean(int address)
        {
            return BitConverter.ToBoolean(ReadBytes(address, 1), 0);
        }

        /// <summary>
        /// Reads a float in the memory of the current process
        /// </summary>
        /// <param name="address">The address where to read the float</param>
        /// <returns>The float at the given address in the memory</returns>
        public float ReadFloat(int address)
        {
            return BitConverter.ToSingle(ReadBytes(address, 4), 0);
        }

        // Called to update the main module address when the module is unloaded
        private void ResetBaseAddress(object sender, EventArgs e)
        {
            _mainModuleLoaded = false;
            _mainModuleAddress = IntPtr.Zero;
        }
    }
}
