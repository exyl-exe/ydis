using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Whydoisuck.Properties;
using Whydoisuck.Utilities;

namespace Whydoisuck.Model.Utilities
{
    public static class ExceptionLogger
    {
        private static string LogsPath => Settings.Default.LogsPath;

        public static void Log(Exception e)
        {
            if (!SafeDirectory.Exists(LogsPath))
            {
                try
                {
                    SafeDirectory.CreateDirectory(LogsPath);
                } catch
                {
                    //Invalid folder, unlucky
                }
            }
            var fileName = string.Format("{0:yyyy}{0:mm}{0:dd}_{0:hh}{0:ss}{0:ffff}", DateTime.Now);
            var text = e.Message + "\n" + e.StackTrace;
            SafeFile.WriteAllText(SafePath.Combine(LogsPath, fileName), text);
        }

    }
}
