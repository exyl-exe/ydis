using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Whydoisuck.ViewModels.CommonViewModels;

namespace Whydoisuck.Views.CommonControls
{
    /// <summary>
    /// Logique d'interaction pour PercentRange.xaml
    /// </summary>
    public partial class PercentRangeSelector : UserControl
    {
        public PercentRangeSelector()
        {
            InitializeComponent();
        }

        private void RangeSlider_LowerValueChanged(object sender, RoutedEventArgs e)
        {
            var viewmodel = (PercentRangeSliderViewModel)DataContext;
            viewmodel?.OnLowerValueChanged(sender, e);
        }

        private void RangeSlider_HigherValueChanged(object sender, RoutedEventArgs e)
        {
            var viewmodel = (PercentRangeSliderViewModel)DataContext;
            viewmodel?.OnHigherValueChanged(sender, e);
        }
    }
}
