using OxyPlot;
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

namespace Whydoisuck.Views.CommonControls
{
    /// <summary>
    /// Code-behind for OxyPlotTooltipHover.xaml
    /// Wrapper of OxyPlot Plot, so it can be used as a control while using a custom tracker.
    /// </summary>
    public partial class OxyPlotTooltipHover : OxyPlot.Wpf.Plot
    {
        public OxyPlotTooltipHover()
        {
            InitializeComponent();
            InitController();
        }

        private void InitController()
        {
            ActualController.UnbindMouseDown(OxyMouseButton.Left);
            ActualController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
        }
    }
}
