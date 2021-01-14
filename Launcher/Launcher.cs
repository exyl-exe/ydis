using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Launcher
{
    /// <summary>
    /// Class that can launch a process automatically when another process is opened
    /// </summary>
    public class Launcher
    {
        private string processToDetect;
        private string processToLaunch;
        private int scanRate;
        private bool shouldStop;

        public Launcher(string applicationToDetect, string processToLaunch, int scanRate)
        {
            this.processToDetect = applicationToDetect;
            this.processToLaunch = processToLaunch;
            this.scanRate = scanRate;
        }

        /// <summary>
        /// Initiates the scanning loop
        /// </summary>
        public void Start()
        {
            shouldStop = false;
            while (!shouldStop)
            {

                Thread.Sleep(scanRate);
            }
        }
    }
}
