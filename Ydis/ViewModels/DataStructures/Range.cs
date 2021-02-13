using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ydis.ViewModels.DataStructures
{
    /// <summary>
    /// Range class that can be used to check if a value is between two other values.
    /// </summary>
    public class Range : IComparable
    {
        /// <summary>
        /// The lower value of the range.
        /// </summary>
        public double Start { get; set; }
        /// <summary>
        /// The higher value of the range.
        /// </summary>
        public double End { get; set; }

        public Range(double Start, double End)
        {
            this.Start = Start;
            this.End = End;
        }

        /// <summary>
        /// Checks if a value belongs to the range
        /// </summary>
        /// <param name="f">The value to check for.</param>
        /// <returns>True if the value is in bounds.</returns>
        public bool Contains(double f)
        {
            return Start <= f && f < End;
        }

        /// <summary>
        /// Compares this range to another.
        /// </summary>
        /// <param name="obj">The range this object will be compared to.</param>
        /// <returns>1 if this range is completely above the other range
        /// -1 if the other range is completely above this range
        /// 0 if the other object is not a range or if this range and the other range overlap</returns>
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
