using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
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

        public GraphView()
        {
            InitializeComponent();
            Update();
        }

        public void Update()
        {
            var groups = GroupLoader.GetAllGroups();
            comboBoxGroups.ItemsSource = groups;

            
            if (groups.Count == 0) return;//TODO group selection
            var group = groups[0];
            var percents = GetLevelPercentsData(group);
            LevelDataGrid.ItemsSource = percents;

            var graphSource = new ChartValues<ObservablePoint>();
            for(int i = 0; i < percents.Count; i++)//TODO foreach ?
            {
                var percent = percents[i];
                graphSource.Add(new ObservablePoint(percent.PercentRange.Start, percent.PassRate));
            }

            LevelChartSerie.Values = graphSource;

            DataContext = this;
        }

        private List<LevelPercentData> GetLevelPercentsData(GroupDisplayer group)//TODO gros bordel
        {
            var rangeWidth = 0.5f;
            var attempts = group.GroupSessions.SelectMany(s => s.Attempts).ToList();
            var attList = GetAttemptsRangeList(attempts, rangeWidth);
            var groupedAttempts = attList.GetContent();

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
