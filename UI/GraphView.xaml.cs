using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
using Whydoisuck.DataSaving;

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
            if (groups.Count == 0) return;
            var group = groups[0];
            var deaths = group.GroupSessions.SelectMany(s => s.Attempts).Select(a => a.EndPercent).ToList();

            Percents = new ChartValues<ObservablePoint>();

            var dic = new Dictionary<int, int>();
            foreach(var d in deaths)
            {
                var key = (int)d;
                if (dic.ContainsKey(key))
                {
                    dic[key]++;
                } else
                {
                    dic[key] = 1;
                }
            }

            var count = 0;
            var keys = new List<int>(dic.Keys);
            keys.Sort();
            keys.Reverse();
            for(int i = 100; i >= 0; i--)
            {
                if (dic.ContainsKey(i))
                {
                    count += dic[i];
                    Percents.Add(new ObservablePoint(i, (float)dic[i]*100f/(float)count));
                } else
                {
                    Percents.Add(new ObservablePoint(i, (float)0));
                }
            }
            DataContext = this;
        }
    }
}
