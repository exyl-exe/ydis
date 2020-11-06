using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Utilities
{
    /// <summary>
    /// File class that manages exceptions.
    /// </summary>
    public class SafeFile
    {
        public static void WriteAllText(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }

        public static bool Exists(string indexFilePath)
        {
            return File.Exists(indexFilePath);
        }

        public static string ReadAllText(string indexFilePath)
        {
            return File.ReadAllText(indexFilePath);
        }
    }
}
