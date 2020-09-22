using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.UIModel.RangeDataStructures
{
    class Range
    {
        public float Start { get; set; }
        public float End { get; set; }

        public static Range operator -(Range r, float f)
        {
            return new Range { Start = r.Start - f, End = r.End - f };
        }

        public static Range operator +(Range r, float f)
        {
            return new Range { Start = r.Start + f, End = r.End + f };
        }
    }
}
