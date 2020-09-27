using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.UIModel.DataStructures
{
    public class RangeAttemptList
    {
        public Range Range { get; set; }
        public List<SessionAttempt> Attempts { get; set; }

        public RangeAttemptList(Range range)
        {
            Range = range;
            Attempts = new List<SessionAttempt>();
        }
    }
}
