using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Whydoisuck.DataSaving;
using Whydoisuck.UIModel;

namespace Whydoisuck.UI
{
    /// <summary>
    /// Logique d'interaction pour GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        public ChartValues<ObservablePoint> Percents { get; set; }
        public CartesianMapper<ObservablePoint> Mapper { get; set; }
        public bool ShowCopyAttempts { get; set; }
        public bool ShowNormalAttempts { get; set; }

        public GraphView()
        {
            InitializeComponent();
            Mapper = Mappers.Xy<ObservablePoint>()
            .X((item, index) => item.X)
            .Y(item => item.Y);
            Update();        
        }

        public void Update()
        {
            var groups = GroupLoader.GetAllGroups();
            comboBoxGroups.ItemsSource = groups;
            
            if (groups.Count == 0) return;//TODO group selection
            var group = groups[0];

            var rangeWidth = 1f;
            var percents = GetLevelPercentsData(group, rangeWidth);

            LevelChartSerie.Values = CreateValuesFromPercents(percents, rangeWidth);
            LevelDataGrid.ItemsSource = percents;

            DataContext = this;
        }

        private void ToggleCopy(object sender, EventArgs e)
        {
            ShowCopyAttempts = (bool)(sender as ToggleButton).IsChecked;
            Update();
        }

        private void ToggleNormal(object sender, EventArgs e)
        {
            ShowNormalAttempts = (bool)(sender as ToggleButton).IsChecked;
            Update();
        }

        private bool SessionSelectCondition(Session s)
        {
            if (ShowCopyAttempts && s.IsCopyRun)
            {
                return true;
            }

            if (ShowNormalAttempts && !s.IsCopyRun)
            {
                return true;
            }

            return false;
        }

        private ChartValues<ObservablePoint> CreateValuesFromPercents(List<LevelPercentData> percents, float rangeWidth)
        {
            var res = new ChartValues<ObservablePoint>();

            if (percents.Count == 0) return res;

            var lastAdded = percents[0];
            res.Add(new ObservablePoint(lastAdded.PercentRange.Start, lastAdded.PassRate));

            for (int i = 1; i < percents.Count; i++)
            {
                var percent = percents[i];

                if (percent.PercentRange.Start - lastAdded.PercentRange.Start > rangeWidth)
                {
                    res.Add(new ObservablePoint(lastAdded.PercentRange.Start + rangeWidth, 100));
                    res.Add(new ObservablePoint(percent.PercentRange.Start - rangeWidth, 100));
                }

                res.Add(new ObservablePoint(percent.PercentRange.Start, percent.PassRate));

                lastAdded = percent;
            }

            return res;
        }

        private List<LevelPercentData> GetLevelPercentsData(GroupDisplayer group, float rangeWidth)//TODO gros bordel
        {
            
            var attempts = group.GroupSessions.Where(s => SessionSelectCondition(s)).SelectMany(s => s.Attempts).ToList();
            var attList = GetAttemptsRangeList(attempts, rangeWidth);
            var groupedAttempts = attList.Content;

            var percents = new List<LevelPercentData>();
            var reachCount = 0;
            for(var i=groupedAttempts.Count-1; i >= 0; i--)
            {
                var attemptGroup = groupedAttempts[i];
                reachCount += attemptGroup.Attempts.Count;
                var currentPercentData = new LevelPercentData
                {
                    PercentRange = attemptGroup.Index,
                    ReachCount = reachCount,
                    DeathCount = attemptGroup.Attempts.Count,
                };
                percents.Add(currentPercentData);
            }

            percents.Sort((p1, p2) =>
            (int) (
                    (p1.PercentRange.Start - p2.PercentRange.Start)*
                    (1/Math.Abs(p1.PercentRange.Start - p2.PercentRange.Start)))
            );
            return percents;
        }

        private AttemptRangeList GetAttemptsRangeList(List<Attempt> attempts, float rangeWidth)
        {
            var list = new AttemptRangeList(rangeWidth);
            list.AddList(attempts);
            return list;
        }
    }
}
