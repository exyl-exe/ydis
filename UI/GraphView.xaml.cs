using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
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
        public CartesianMapper<ObservablePoint> Mapper { get { return GetMapper(); } }

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

            var rangeWidth = 1f;
            var percents = GetLevelPercentsData(group, rangeWidth);
            LevelDataGrid.ItemsSource = percents;

            LevelChartSerie.Configuration = GetMapper();
            LevelChartSerie.Values = CreateValuesFromPercents(percents, rangeWidth);
            
            DataContext = this;
        }

        private static CartesianMapper<ObservablePoint> GetMapper()
        {
            return Mappers.Xy<ObservablePoint>()
            .X((item, index) => item.X)
            .Y(item => item.Y)
            /*.Fill(item => item.Y >=100 ? Brushes.Transparent : null)
            .Stroke(item => item.Y >= 100 ? Brushes.Transparent : null)*/;
        }

        private ChartValues<ObservablePoint> CreateValuesFromPercents(List<LevelPercentData> percents, float rangeWidth)
        {
            var res = new ChartValues<ObservablePoint>();

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
            //list.FillEmpty();
            return list;
        }
    }
}
