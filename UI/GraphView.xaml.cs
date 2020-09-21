using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
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
            foreach(var percent in percents)
            {
                graphSource.Add(new ObservablePoint(percent.Percent, percent.PassRate));
            }

            LevelChartSerie.Values = graphSource;

            DataContext = this;
        }

        private List<LevelPercentData> GetLevelPercentsData(GroupDisplayer group)
        {
            var attempts = group.GroupSessions.SelectMany(s => s.Attempts).Select(a => a.EndPercent).ToList();
            var attDic = GetAttemptsDictionnary(attempts);
            var keys = new List<int>(attDic.Keys);
            keys.Sort();

            var percents = new List<LevelPercentData>();
            var reachCount = 0;
            for(var i=keys.Count-1; i >= 0; i--)
            {
                var percent = keys[i];
                reachCount += attDic[percent];
                var currentPercentData = new LevelPercentData
                {
                    Percent = percent,
                    ReachCount = reachCount,
                    DeathCount = attDic[percent],
                };
                percents.Add(currentPercentData);
            }

           /* for(int i = 0; i < 100; i++)
            {
                if (!attDic.ContainsKey(i))
                {
                    var currentPercentData = new LevelPercentData
                    {
                        Percent = i,
                        ReachCount = 1,
                        DeathCount = 0,
                    };
                    percents.Add(currentPercentData);
                }
            }
            percents.Sort((p1, p2) => p1.Percent >= p2.Percent?1:-1);*/
            return percents;
        }

        private Dictionary<int,int> GetAttemptsDictionnary(List<float> attempts)
        {
            var dic = new Dictionary<int, int>();
            for (int i = 0; i < attempts.Count; i++)
            {
                var key = (int)attempts[i];
                if (dic.ContainsKey(key))
                {
                    dic[key]++;
                }
                else
                {
                    dic[key] = 1;
                }
            }
            return dic;
        }
    }
}
