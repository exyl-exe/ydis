using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;
using Whydoisuck.ModelAgain;
using Whydoisuck.UIModel;

namespace Whydoisuck.ViewModels.CommonViewModels
{
    public class LevelDataGridViewModel : BaseViewModel
    {
        public List<LevelPartStatistics> SessionStats { get; set; }
        public LevelDataGridViewModel(List<LevelPartStatistics> stats)
        {
            SessionStats = FilterOutRedondantParts(stats);
        }

        public List<LevelPartStatistics> FilterOutRedondantParts(List<LevelPartStatistics> stats)
        {
            var res = new List<LevelPartStatistics>();
            if (stats.Count > 0)
            {
                var prec = stats[0];
                if (prec.DeathCount > 0) res.Add(prec);//showing first element only if it brings useful information
                foreach (var part in stats)
                {
                    if (part.DeathCount != 0 && part.DeathCount != prec.DeathCount)
                    {
                        res.Add(part);
                        prec = part;
                    }
                }
            }
            return res;
        }
    }
}
