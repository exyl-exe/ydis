using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.DataSaving
{
    public class Attempt
    {
        public int Number { get; set; }
        public float EndPercent { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        public int Compare(Attempt attempt)
        {
            if (EndPercent > attempt.EndPercent)
            {
                return 1;
            }
            else if(EndPercent < attempt.EndPercent)
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
