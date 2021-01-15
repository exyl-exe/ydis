using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Environment;

namespace Whydoisuck.Model.Utilities
{
    /// <summary>
    /// Contains a few methods to help write/read applications that should be launched when windows is starting
    /// </summary>
    public class AutoLaunchUtilities
    {
        private readonly static string StartupFolder = GetFolderPath(SpecialFolder.Startup);
        /// <summary>
        /// Creates a shortcut so that an application is launched on windows' start up
        /// </summary>
        public static void CreateStartUpShortcut(string shortcutName, string applicationPath)
        {
            var appFullPath = Path.GetFullPath(applicationPath);
            using (StreamWriter writer = new StreamWriter(Path.Combine(StartupFolder, shortcutName)))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + appFullPath);
            }
        }

        /// <summary>
        /// Checks wether an application is already launched on windows' start up
        /// </summary>
        public static bool DoesShortcutExist(string shortcutName, string applicationPath)
        {
            var shortcutPath = Path.Combine(StartupFolder, shortcutName);
            var appFullPath = Path.GetFullPath(applicationPath);
            if (!File.Exists(shortcutPath)) return false;
            return File.ReadAllText(shortcutPath).Contains(appFullPath); // branle
        }

        /// <summary>
        /// Removes the shortcut with the given name from the start up directory
        /// </summary>
        public static void RemoveStartUpShortcut(string shortcutName)
        {
            var shortcutPath = Path.Combine(StartupFolder, shortcutName);
            if (File.Exists(shortcutPath)) File.Delete(shortcutPath);
        }
    }
}
