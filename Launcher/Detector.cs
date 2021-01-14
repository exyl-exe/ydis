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

        public bool WasLaunched()
        {
            return false;
        }
    }
}
