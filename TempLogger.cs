using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck
{
    static class TempLogger//TODO remove file
    {
        static string Logs;
        /*public delegate void LogEventHandler(string s);
        public static event LogEventHandler LogEvent;*/

        public static string Flush()
        {
           // LogEvent.Invoke(Logs);
            var res = Logs;
            Logs = "-- Logs --";
            return res;
        }

        public static void AddLog(string log)
        {
            Logs += "\n";
            Logs += log;
        }
    }
}
