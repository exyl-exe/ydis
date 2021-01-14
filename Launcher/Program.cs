using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WDISLauncher
{
    public class Program
    {
        const string processToDetect = "GeometryDash";
        const string processToLaunch = "";
        const int scanRate = 3000;
        static void Main(string[] args)
        {
            var launcher = new Launcher(processToDetect, processToLaunch, scanRate);
            launcher.Start();
        }
    }
}
