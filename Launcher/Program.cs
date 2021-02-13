using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YdisLauncher
{
    public class Program
    {
        const string MUTEX_NAME = "WDISLauncherMutex";
        const string PROCESS_TO_DETECT = "GeometryDash";
        readonly static string processToLaunch = WDISPathGetter.GetWDISPath();
        const int scanRate = 3000;
        private static Mutex _mutex;

        public static void Main()
        {
            _mutex = new Mutex(true, MUTEX_NAME, out var createdNew);
            if (!createdNew) return;

            if (File.Exists(processToLaunch))
            {
                var launcher = new Launcher(PROCESS_TO_DETECT, processToLaunch, scanRate);
                launcher.Start();
            }
        }
    }
}
