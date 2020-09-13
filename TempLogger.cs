using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck
{
    static class TempLogger//TODO remove file
    {
        static List<string> Logs = new List<string>();
        /*public delegate void LogEventHandler(string s);
        public static event LogEventHandler LogEvent;*/

        public static string Flush()
        {
           // LogEvent.Invoke(Logs);
            var res = "--Logs--\n";
            foreach(var log in Logs)
            {
                res += log;
            }
            Logs.Clear();
            return res;
        }

        public static string FlushLast()
        {
            if (Logs.Count == 0) return "No logs";
            var res = Logs[Logs.Count - 1];
            Logs.Clear();
            return res;
        }

        public static void AddLog(string log)
        {
            Logs.Add("\n");
            Logs.Add(log);
        }
    }
}
