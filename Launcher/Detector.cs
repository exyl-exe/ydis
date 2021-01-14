using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDISLauncher
{
    /// <summary>
    /// Class that can be used to check if a given application has been launched
    /// </summary>
    public class Detector
    {
        private string applicationToWatchFor;
        private Process currentProcess;

        public Detector(string applicationToWatchFor)
        {
            this.applicationToWatchFor = applicationToWatchFor;
        }

        /// <summary>
        /// Checks if the app was launched
        /// </summary>
        public bool AppWasLaunched()
        {
            if (currentProcess != null && !currentProcess.HasExited) return false;
            var matchingProcesses = Process.GetProcessesByName(applicationToWatchFor);
            var appOpened = matchingProcesses.Length != 0;
            currentProcess = appOpened ? matchingProcesses[0] : null;
            return appOpened;
        }
    }
}
