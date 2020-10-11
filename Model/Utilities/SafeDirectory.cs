using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Utilities
{
    class SafeDirectory
    {
        public static string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        public static string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public static bool Exists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public static void CreateDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}
