using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Whydoisuck.UIModel.GraphView;
using Xceed.Wpf.Toolkit;

namespace Whydoisuck.ViewModels.CommonViewModels
{
    public class PercentRangeSliderViewModel : BaseViewModel
    {
        public int MAX { get; } = 100;
        public int MIN { get; } = 0;

        public int HigherValue { get => (int)Range.End; set => OnHigherValueChanged(value); }
        public int LowerValue { get => (int)Range.Start; set => OnLowerValueChanged(value); }

        public Range Range { get; }

        public event EventHandler OnRangeChanged;

        public PercentRangeSliderViewModel()
        {
            Range = new Range(MIN, MAX);
        }

        public void OnLowerValueChanged(double newValue)
        {
           if (Range.Start != newValue)
           {
               Range.Start = newValue;
               OnRangeChanged?.Invoke(this, null);
           }
        }

        public void OnHigherValueChanged(double newValue)
        {
            if (Range.End != newValue)
            {
                Range.End = newValue;
                OnRangeChanged?.Invoke(this, null);
            }
        }
    }
}
