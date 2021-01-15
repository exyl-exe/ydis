using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDISLauncher
{
    /// <summary>
    /// Class that fetches where the whydoisuck executable is
    /// </summary>
    public class WDISPathGetter
    {
        public static string AppData { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Whydoisuck");
        public static string AppLocationInfo { get; } = Path.Combine(AppData, "path.dat");
        public static string GetWDISPath()
        {
            if (File.Exists(AppLocationInfo))
            {
                return File.ReadAllText(AppLocationInfo);
            } else
            {
                return null;
            }
        }
    }
}
