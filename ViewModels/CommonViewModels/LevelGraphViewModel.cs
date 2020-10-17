using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.UIModel;

namespace Whydoisuck.ViewModels.CommonViewModels
{
    public class LevelGraphViewModel
    {
        private List<LevelPartStatistics> Statistics { get; set; }
        public List<DataPoint> Points { get; set; }

        public LevelGraphViewModel(List<LevelPartStatistics> statistics)
        {
            Statistics = statistics;
            Points = GetPoints(statistics);
        }

        private List<DataPoint> GetPoints(List<LevelPartStatistics> stats)
        {
            var res = new List<DataPoint>();

            if(stats.Count != 0)
            {
                res.Add(new DataPoint(stats[0].PercentRange.Start, stats[0].PassRate));
                for (var i = 1; i < stats.Count-1; i++)
                {
                    if(stats[i].PassRate != stats[i-1].PassRate || stats[i].PassRate != stats[i + 1].PassRate)//TODO epsilon ?
                    {
                        res.Add(new DataPoint(stats[i].PercentRange.Start, stats[i].PassRate));
                    }  
                }
                res.Add(new DataPoint(stats[stats.Count-1].PercentRange.Start, stats[stats.Count - 1].PassRate));
            }
            return res;
        }
    }
}
