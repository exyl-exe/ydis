using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.UIModel.GraphView
{
    public class Range : IComparable
    {
        public double Start { get; set; }
        public double End { get; set; }

        public Range(double Start, double End)
        {
            this.Start = Start;
            this.End = End;
        }

        public bool Contains(double f)
        {
            return Start <= f && f < End;
        }

        public bool GreaterEquals(double f)
        {
            return f < End;
        }

        public int CompareTo(object obj)
        {
            if(obj is Range other)
            {
                if (other.Start >= End)
                {
                    return -1;
                }

                if(other.End <= Start)
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}
