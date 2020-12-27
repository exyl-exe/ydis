using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Whydoisuck.Model.UserSettings;
using Whydoisuck.Properties;

namespace Whydoisuck.Model.Utilities
{
    public static class ExceptionLogger
    {
        private static string LogsPath => WDISSettings.LogsPath;

        public static void Log(Exception e)
        {
            if (!Directory.Exists(LogsPath))
            {
                try
                {
                    Directory.CreateDirectory(LogsPath);
                } catch
                {
                    //Invalid folder, unlucky
                }
            }
            var fileName = string.Format("{0:yyyy}{0:mm}{0:dd}_{0:hh}{0:ss}{0:ffff}", DateTime.Now);
            var text = e.Message + "\n" + e.StackTrace;
            File.WriteAllText(Path.Combine(LogsPath, fileName), text);
        }

    }
}
