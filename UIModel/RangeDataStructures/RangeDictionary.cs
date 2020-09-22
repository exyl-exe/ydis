using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;

namespace Whydoisuck.UIModel.RangeDataStructures
{
    class RangeDictionary<TKey, TValue>
    {
        public int Count { get { return Content.Count; }}

        private List<RangeDictionaryElement<TValue>> Content { get; set; } = new List<RangeDictionaryElement<TValue>>();
        private float RangeWidth { get; set; }//entries will be [0,KeyWidth][KeyWidth,2*KeyWidth][2*KeyWidth,3*KeyWidth] ...
        private Func<TKey, float> KeySelector { get; set; }

        public RangeDictionary(float rangeWidth, Func<TKey, float> KeySelector)
        {
            this.RangeWidth = rangeWidth;
            this.KeySelector = KeySelector;
        }

        public RangeDictionaryElement<TValue> At(int index)
        {
            return Content[index];
        }

        public TValue Get(TKey element)
        {
            var range = SearchRangeFor(KeySelector(element), out var pos);
            if(range == null)
            {
                return default(TValue);
            } else
            {
                return range.Element;
            }
        }

        public void Add(TKey key, TValue element)
        {
            var targetRange = SearchRangeFor(KeySelector(key), out var pos);
            if (targetRange != null)
            {
                targetRange.Element=element;
            } else
            {
                var beginOfRange = GetRangeStart(KeySelector(key));
                var range = new Range()
                {
                    Start = beginOfRange,
                    End = beginOfRange + RangeWidth
                };
                var newElement = new RangeDictionaryElement<TValue>(range, element);
                Content.Insert(pos, newElement);
            }
        }

        private RangeDictionaryElement<TValue> SearchRangeFor(float key, out int pos){

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

    class RangeDictionaryElement<T>
    {
        public Range Range { get; set; }
        public T Element { get; set; }

        public RangeDictionaryElement(Range range, T Element)
        {
            this.Range = range;
            this.Element = Element;
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
}
