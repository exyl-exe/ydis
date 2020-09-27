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
using Whydoisuck.UIModel;
using Whydoisuck.UIModel.DataStructures;

namespace Whydoisuck.UI
{
    /// <summary>
    /// Logique d'interaction pour GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        private const int DEFAULT_PASS_RATE = 100;

        private bool NormalToggle { get; set; }
        private bool CopyToggle { get; set; }

        public CartesianMapper<ObservablePoint> Mapper { get; set; }
        private AttemptGraph CurrentGraph { get; set; }

        public GraphView()
        {
            InitializeComponent();
            RefreshGroups();//TODO proper refresh
            Mapper = Mappers.Xy<ObservablePoint>()
            .X((item, index) => item.X)
            .Y(item => item.Y);
        }

        public void UpdateGraph()//TODO clean way to update when a value changes
        {
            if (CurrentGraph == null) return;
            var rangeWidth = 1f;
            CurrentGraph.Filter.ShowCopyRuns = CopyToggle;
            CurrentGraph.Filter.ShowNormalRuns = NormalToggle;
            var percents = CurrentGraph.GetLevelPercentsData(rangeWidth);
            LevelDataGrid.ItemsSource = percents;
            LevelChart.Values = CreateChartValues(percents, rangeWidth);
            DataContext = this;
        }

        private ChartValues<ObservablePoint> CreateChartValues(List<LevelPercentData> percents, float rangeWidth)
        {
            var res = new ChartValues<ObservablePoint>();

            LevelPercentData lastAdded = null;
            foreach(var percent in percents)
            {
                AddIntermediateValues(res, lastAdded, percent, rangeWidth);
                res.Add(new ObservablePoint(percent.PercentRange.Start, percent.PassRate));
                lastAdded = percent;
            }
            return res;
        }

        private void AddIntermediateValues(ChartValues<ObservablePoint> values, LevelPercentData lastAdded, LevelPercentData current, float rangeWidth)
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
            CopyToggle = (bool)(sender as ToggleButton).IsChecked;
            UpdateGraph();
        }

        private void ToggleNormal(object sender, EventArgs e)
        {
            NormalToggle = (bool)(sender as ToggleButton).IsChecked;
            UpdateGraph();
        }

        private void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void ComboBoxGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var group= comboBoxGroups.SelectedItem as GroupDisplayer;
            if (group == null) return;
            CurrentGraph = new AttemptGraph(group, new SessionFilter(NormalToggle, CopyToggle));//TODO use current selection filter
            UpdateGraph();
        }

        private void RefreshGroupsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RefreshGroups();
        }

        private void RefreshGroups()
        {
            var groups = GroupLoader.GetAllGroups();
            comboBoxGroups.ItemsSource = groups;
        }
    }
}
