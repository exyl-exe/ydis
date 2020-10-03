using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel.SerializedData;

namespace Whydoisuck.DataModel
{
    public class Attempt
    {
        public int Number { get; set; }
        public float EndPercent { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        public Attempt() { }

        public Attempt(SerializedAttempt a)
        {
            Number = a.Number;
            EndPercent = a.EndPercent;
            StartTime = a.StartTime;
            Duration = a.Duration;
        }

        public int Compare(Attempt attempt)
        {
            if (EndPercent > attempt.EndPercent)
            {
                return 1;
            }
            else if (EndPercent < attempt.EndPercent)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
