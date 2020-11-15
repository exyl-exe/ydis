using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Whydoisuck.Properties;
using Whydoisuck.ViewModels.DataStructures;

namespace Whydoisuck.ViewModels.CommonControlsViewModels
{
    /// <summary>
    /// View model for a graph showing statistics about a level
    /// </summary>
    public class LevelGraphViewModel : BaseViewModel
    {
        /// <summary>
        /// Points in the graph.
        /// </summary>
        public List<LevelPartDataPoint> Points { get; set; }

        /// <summary>
        /// Title of the graph
        /// </summary>
        public string GraphTitle { get; private set; }

        // Statistics depicted by the graph.
        private SessionsStatistics Statistics { get; set; }

        public LevelGraphViewModel(SessionsStatistics statistics, string title)
        {
            Statistics = statistics;
            GraphTitle = title;
            Points = GetPoints(statistics.Statistics);
            Statistics.OnStatisticsChange += Update;
        }

        // Notifies the view about a change
        private void Update()
        {
            Points = GetPoints(Statistics.Statistics);
            OnPropertyChanged(nameof(Points));
        }

        // Creates points for the given statistics
        private List<LevelPartDataPoint> GetPoints(List<LevelPartStatistics> stats)
        {
            var res = new List<LevelPartDataPoint>();
            if(stats.Count != 0)
            {
                res.Add(new LevelPartDataPoint(stats[0]));
                for (var i = 1; i < stats.Count-1; i++)
                {
                    if(stats[i].PassRate != stats[i-1].PassRate || stats[i].PassRate != stats[i + 1].PassRate)//TODO epsilon ?
                    {
                        res.Add(new LevelPartDataPoint(stats[i]));
                    }  
                }
                res.Add(new LevelPartDataPoint(stats[stats.Count - 1]));
            }
            return res;
        }
    }
}
