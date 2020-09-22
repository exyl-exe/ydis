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
        public static float KEY_EPSILON = 0.001f;
        public int Count { get { return Content.Count; }}

        private List<SelectDictionaryKeyValue<float, TValue>> Content { get; set; } = new List<SelectDictionaryKeyValue<float, TValue>>();
        private Func<TKey, float> KeySelector { get; set; }

        public RangeDictionary(Func<TKey, float> KeySelector)
        {
            this.KeySelector = KeySelector;
        }

        public TValue At(int index)
        {
            return Content[index].Value;
        }

        public void SetAt(int index, TValue value)
        {
            Content[index].Value = value;
        }

        public TValue Get(TKey element)
        {
            var res = SearchFor(KeySelector(element), out var pos);
            if(res == null)
            {
                return default;//TODO
            } else
            {
                return res.Value;
            }
        }

        public TValue Get(float value)
        {
            var res = SearchFor(value, out var pos);
            if (res == null)
            {
                return default;//TODO
            }
            else
            {
                return res.Value;
            }
        }

        public SelectDictionaryKeyValue<float, TValue> GetPair(float value)
        {
            var res = SearchFor(value, out var pos);
            if (res == null)
            {
                return default;//TODO
            }
            else
            {
                return res;
            }
        }

        public void Affect(TKey key, TValue element)
        {
            var target = SearchFor(KeySelector(key), out var pos);
            if (target != null)
            {
                target.Value=element;
            } else
            {
                var newElement = new SelectDictionaryKeyValue<float, TValue>(KeySelector(key), element);
                Content.Insert(pos, newElement);
            }
        }

        private SelectDictionaryKeyValue<float, TValue> SearchFor(float key, out int pos){

            if (Content.Count == 0)
            {
                pos = 0;
                return default;
            }

            int inf = 0;
            int sup = Content.Count-1;
            int middle = 0;
            while (inf<=sup)
            {
                middle = (inf + sup) / 2;
                var middleKey = Content[middle].Key;
                if (Math.Abs(middleKey-key)<KEY_EPSILON)// ==
                {
                    pos = middle;
                    return Content[middle];
                } else if (middleKey+KEY_EPSILON<key)// <
                {
                    inf = middle+1;
                } else// >
                {
                    sup = middle-1;
                }
            }

            pos = Content[middle].Key > key+KEY_EPSILON ? middle : middle + 1;
            return null;
        }
    }

    class SelectDictionaryKeyValue<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public SelectDictionaryKeyValue(TKey Key, TValue Value)
        {
            this.Key = Key;
            this.Value = Value;
        }
    }
}
