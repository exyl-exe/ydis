using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;

namespace Whydoisuck.UIModel
{
    class AttemptRangeList
    {
        private List<AttemptRangeElement> Content { get; set; } = new List<AttemptRangeElement>();
        private float RangeWidth { get; set; }//entries will be [0,KeyWidth][KeyWidth,2*KeyWidth][2*KeyWidth,3*KeyWidth] ...

        public AttemptRangeList(float rangeWidth)
        {
            this.RangeWidth = rangeWidth;
        }

        public void Add(Attempt attempt)
        {
            var targetRange = SearchRangeFor(attempt.EndPercent, out var pos);
            if (targetRange != null)
            {
                targetRange.AddAttempt(attempt);
            } else
            {
                var beginOfRange = GetRangeStart(attempt.EndPercent);
                var range = new Range()
                {
                    Start = beginOfRange,
                    End = beginOfRange + RangeWidth
                };
                var newElement = new AttemptRangeElement(range);
                newElement.AddAttempt(attempt);
                Content.Insert(pos, newElement);
            }
        }

        private AttemptRangeElement SearchRangeFor(float key, out int pos){

            if (Content.Count == 0)
            {
                pos = 0;
                return null;
            }

            int inf = 0;
            int sup = Content.Count-1;
            int middle = 0;
            while (inf<=sup)
            {
                middle = (inf + sup) / 2;
                if (Content[middle].InBounds(key))
                {
                    pos = middle;
                    return Content[middle];
                } else if (Content[middle].RangeBelow(key))
                {
                    inf = middle+1;
                } else
                {
                    sup = middle-1;
                }
            }

            pos = Content[middle].RangeAbove(key) ? middle : middle + 1;
            return null;
        }

        private float GetRangeStart(float value)
        {
            return ((int)(value/RangeWidth))*RangeWidth;
        }

    }

    class AttemptRangeElement
    {
        public Range Index { get; set; }
        public List<Attempt> Attempts { get; set; } = new List<Attempt>();

        public AttemptRangeElement(Range range)
        {
            this.Index = range;
        }

        public void AddAttempt(Attempt att)
        {
            Attempts.Add(att);
        }

        public bool RangeBelow(float value)
        {
            return Index.End <= value;
        }

        public bool RangeAbove(float value)
        {
            return value < Index.Start;
        }

        public bool InBounds(float value)
        {
            return Index.Start <= value && value < Index.End;
        }
    } 

    class Range
    {
        public float Start { get; set; }
        public float End { get; set; }
    }
}
