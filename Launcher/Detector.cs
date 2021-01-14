using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
    public class Detector
    {
        private string applicationToWatchFor;
        private Process currentProcess;

        public Detector(string processToWatchFor)
        {
            this.applicationToWatchFor = processToWatchFor;
        }

        /// <summary>
        /// Checks if the app was launched
        /// </summary>
        /// <returns></returns>
        public bool AppWasLaunched()
        {
            if (currentProcess != null && !currentProcess.HasExited) return false;
            var matchingProcesses = Process.GetProcessesByName(applicationToWatchFor);
            var appOpened = matchingProcesses.Length == 0;
            currentProcess = appOpened ? matchingProcesses[0] : null;
            return appOpened;
        }
    }
}
