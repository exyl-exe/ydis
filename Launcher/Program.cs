using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WDISLauncher
{
    public class Program
    {
        const string processToDetect = "GeometryDash";
        readonly static string processToLaunch = WDISPathGetter.GetPath();
        const int scanRate = 3000;
        static void Main()
        {
            if (File.Exists(processToLaunch))
            {
                var launcher = new Launcher(processToDetect, processToLaunch, scanRate);
                launcher.Start();
            }
        }
    }
}
