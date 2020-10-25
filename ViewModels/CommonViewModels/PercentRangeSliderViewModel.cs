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
        public Range Range { get; }

        public event EventHandler OnRangeChanged;

        public PercentRangeSliderViewModel()
        {
            Range = new Range(0, 100);
        }

        public void OnLowerValueChanged(object sender, EventArgs e)
        {
            if(sender is RangeSlider rs)
            {
                if(Range.Start != rs.LowerValue)
                {
                    Range.Start = rs.LowerValue;
                    OnRangeChanged?.Invoke(this, null);
                }
            }
        }

        public void OnHigherValueChanged(object sender, EventArgs e)
        {
            if (sender is RangeSlider rs)
            {
                if (Range.End != rs.HigherValue)
                {
                    Range.End = rs.HigherValue;
                    OnRangeChanged?.Invoke(this, null);
                }
            }
        }
    }
}
