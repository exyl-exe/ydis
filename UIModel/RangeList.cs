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
    class RangeList<T>
    {
        public List<RangeElement<T>> Content { get; private set; } = new List<RangeElement<T>>();
        private float RangeWidth { get; set; }//entries will be [0,KeyWidth][KeyWidth,2*KeyWidth][2*KeyWidth,3*KeyWidth] ...
        private Func<T, float> Selector { get; set; }

        public RangeList(float rangeWidth, Func<T, float> Selector)
        {
            this.RangeWidth = rangeWidth;
            this.Selector = Selector;
        }

        public void AddList(List<T> elements)
        {
            foreach(var e in elements)
            {
                Add(e);
            }
        }

        public void Add(T element)
        {
            var targetRange = SearchRangeFor(Selector(element), out var pos);
            if (targetRange != null)
            {
                targetRange.AddElement(element);
            } else
            {
                var beginOfRange = GetRangeStart(Selector(element));
                var range = new Range()
                {
                    Start = beginOfRange,
                    End = beginOfRange + RangeWidth
                };
                var newElement = new RangeElement<T>(range);
                newElement.AddElement(element);
                Content.Insert(pos, newElement);
            }
        }

        private RangeElement<T> SearchRangeFor(float key, out int pos){

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

    class RangeElement<T>
    {
        public Range Range { get; set; }
        public List<T> Elements { get; set; } = new List<T>();

        public RangeElement(Range range)
        {
            this.Range = range;
        }

        public void AddElement(T element)
        {
            Elements.Add(element);
        }

        public bool RangeBelow(float value)
        {
            return Range.End <= value;
        }

        public bool RangeAbove(float value)
        {
            return value < Range.Start;
        }

        public bool InBounds(float value)
        {
            return Range.Start <= value && value < Range.End;
        }
    } 

    class Range
    {
        public float Start { get; set; }
        public float End { get; set; }
        
        public static Range operator-(Range r,float f)
        {
            return new Range { Start = r.Start - f, End = r.End - f };
        }

        public static Range operator +(Range r, float f)
        {
            return new Range { Start = r.Start + f, End = r.End + f };
        }
    }
}
