using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.UIModel.DataStructures
{
    public class Range
    {
        public float Start { get; set; }
        public float End { get; set; }

        public Range(float Start, float End)
        {
            this.Start = Start;
            this.End = End;
        }

        public bool Contains(float f)
        {
            return Start <= f && f < End;
        }

        public bool GreaterEquals(float f)
        {
            return f < End;
        }
    }
}
