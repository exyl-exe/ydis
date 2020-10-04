using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Whydoisuck.DataSaving;
using Whydoisuck.DataModel;
using Whydoisuck.UIModel;
using System.Diagnostics;

namespace Whydoisuck.UI
{
    public partial class GraphView : UserControl
    {
        public static DependencyProperty GroupProperty =
            DependencyProperty.Register("GraphViewSessionGroup",
                                        typeof(SessionGroup),
                                        typeof(GraphView),
                                        new PropertyMetadata(DataChangeCallback));
        public SessionGroup GraphViewSessionGroup
        {
            get { return (SessionGroup)GetValue(GroupProperty); }
            set
            {
                SetValue(GroupProperty, value);
            }
        }

        private static GraphView instance;

        private const int DEFAULT_PASS_RATE = 100;
        public CartesianMapper<ObservablePoint> Mapper { get; set; }
        private AttemptGraph CurrentGraph { get; set; }
        private SessionFilter Filter { get; set; } = new SessionFilter(false, false);
        private float RangeWidth { get; set; } = 1f;

        public GraphView()
        {
            instance = this;
            InitializeComponent();
            Mapper = Mappers.Xy<ObservablePoint>()
            .X((item, index) => item.X)
            .Y(item => item.Y);
        }

        private static void DataChangeCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var value = (SessionGroup)args.NewValue;
            if (value == null && instance != null) return;

            instance.CurrentGraph = new AttemptGraph(value);
            instance.UpdateGraph();
        }


        public void UpdateGraph()
        {
            if (CurrentGraph == null) return;
            var percents = CurrentGraph.GetLevelPercentsData(Filter, RangeWidth);
            LevelDataGrid.ItemsSource = percents;
            LevelChart.Values = CreateChartValues(percents, RangeWidth);
            DataContext = this;
        }

        private ChartValues<ObservablePoint> CreateChartValues(List<LevelPercentData> percents, float rangeWidth)
        {
            var res = new List<ObservablePoint>();

            LevelPercentData lastAdded = null;
            foreach (var percent in percents)
            {
                AddIntermediateValues(res, lastAdded, percent, rangeWidth);
                res.Add(new ObservablePoint(percent.PercentRange.Start, percent.PassRate));
                lastAdded = percent;
            }
            res.Sort((o, o2) => (int)((o.X - o2.X) / Math.Abs(o.X - o2.X)));
            return new ChartValues<ObservablePoint>(res);
        }

        private void AddIntermediateValues(List<ObservablePoint> values, LevelPercentData lastAdded, LevelPercentData current, float rangeWidth)
        {
            if (lastAdded == null) return;
            if (!current.IsNext(lastAdded, rangeWidth))
            {
                var nextOfLast = lastAdded.PercentRange.Start + rangeWidth;
                var previousOfCurrent = current.PercentRange.Start - rangeWidth;

                values.Add(new ObservablePoint(nextOfLast, DEFAULT_PASS_RATE));
                //Don't add a second point if it's the end of the level or if there is a single middle point between values
                if (!current.PercentRange.Contains(100f) && !(Math.Abs(nextOfLast - previousOfCurrent) < (rangeWidth / 2f)))
                {
                    values.Add(new ObservablePoint(previousOfCurrent, DEFAULT_PASS_RATE));
                }
            }
        }

        private void ToggleCopy(object sender, EventArgs e)
        {
            Filter.ShowCopyRuns = (bool)(sender as ToggleButton).IsChecked;
            UpdateGraph();
        }

        private void ToggleNormal(object sender, EventArgs e)
        {
            Filter.ShowNormalRuns = (bool)(sender as ToggleButton).IsChecked;
            UpdateGraph();
        }

        private void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
