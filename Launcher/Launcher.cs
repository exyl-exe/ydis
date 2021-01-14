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
        private Detector appDetector;
        private string processToLaunch;
        private int scanRate;
        private bool shouldStop;

        public Launcher(string applicationToDetect, string processToLaunch, int scanRate)
        {
            this.appDetector = new Detector(applicationToDetect);
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
                if (appDetector.AppWasLaunched())
                {
                    StartProcess();
                }
                Thread.Sleep(scanRate);
            }
        }

        /// <summary>
        /// Starts the application that was given the the launcher
        /// </summary>
        public void StartProcess()
        {
            Console.WriteLine("TODO : WDIS would be launched");
        }
    }
}
