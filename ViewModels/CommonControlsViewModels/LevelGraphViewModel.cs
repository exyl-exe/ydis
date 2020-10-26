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
using Whydoisuck.ViewModels.DataStructures;

namespace Whydoisuck.ViewModels.CommonControlsViewModels
{
    public class LevelGraphViewModel
    {
        private List<LevelPartStatistics> Statistics { get; set; }
        public List<LevelPartDataPoint> Points { get; set; }

        public LevelGraphViewModel(List<LevelPartStatistics> statistics)
        {
            Statistics = statistics;
            Points = GetPoints(statistics);
        }

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
