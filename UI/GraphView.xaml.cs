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
using Whydoisuck.UIModel.DataStructures;

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

            var rangeWidth = 0.1f;
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

            var epsilon = rangeWidth / 2;
            for (int i = 1; i < percents.Count; i++)
            {
                var percent = percents[i];
                if (percent.PercentRange.Start - lastAdded.PercentRange.Start > rangeWidth+epsilon)
                {
                    res.Add(new ObservablePoint(lastAdded.PercentRange.Start + rangeWidth, 100));
                    res.Add(new ObservablePoint(percent.PercentRange.Start - rangeWidth, 100));
                }
                res.Add(new ObservablePoint(percent.PercentRange.Start, percent.PassRate));
                lastAdded = percent;
            }

            return res;
        }

        private List<LevelPercentData> GetLevelPercentsData(GroupDisplayer group, float rangeWidth)//TODO défintivement un gros bordel
        {
            var sessions = group.GroupSessions.Where(s => SessionSelectCondition(s)).ToList();
            var attempts = sessions
                .SelectMany(s => s.Attempts.Select(a => new SessionAttempt() { Attempt = a, Session = s }).ToList())
                .ToList();

            sessions.Sort((s, s2) => (int)((s.StartPercent - s2.StartPercent)/Math.Abs(s.StartPercent - s2.StartPercent)));
            var sessionIndex = 0;

            var attDictionary = GetAttemptRangeList(attempts, rangeWidth);
            var percents = new List<LevelPercentData>();
            var reachCount = 0;
            for(var i = 0; i < attDictionary.Count; i++)
            {
                var attemptsOfGroup = attDictionary.At(i);
                var range = GetRange(attemptsOfGroup[0].Attempt.EndPercent, rangeWidth);

                if (sessionIndex < sessions.Count
                    && (range.Contains(sessions[sessionIndex].StartPercent) || sessions[sessionIndex].StartPercent < range.Start))
                {
                    reachCount += sessions[sessionIndex].Attempts.Count;
                    sessionIndex++;
                }

                var deathCount = attemptsOfGroup.Count;

                var currentPercentData = new LevelPercentData
                {
                    PercentRange = range,
                    ReachCount = reachCount,
                    DeathCount = deathCount,
                };
                percents.Add(currentPercentData);

                reachCount -= deathCount;
            }
            percents.Sort((p1, p2) => p1.Compare(p2));
            return percents;
        }

        private SelectDictionary<SessionAttempt, List<SessionAttempt>> GetAttemptRangeList(List<SessionAttempt> attempts, float rangeWidth)
        {
            var dictionary = new SelectDictionary<SessionAttempt, List<SessionAttempt>>((sa) => GetRange(sa.Attempt.EndPercent, rangeWidth).Start);
            foreach(var a in attempts)
            {
                var attemptList = dictionary.Get(a);
                if(attemptList == null)
                {
                    dictionary.Affect(a, new List<SessionAttempt>() { a });
                } else
                {
                    attemptList.Add(a);
                }
            }
            return dictionary;
        }

        private Range GetRange(float value, float rangeWidth)
        {
            var start = ((int)(value / rangeWidth)) * rangeWidth;
            return new Range(start,start+rangeWidth);
        }
    }
}
