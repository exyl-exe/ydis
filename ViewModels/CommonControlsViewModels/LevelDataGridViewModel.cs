﻿using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.ViewModels.DataStructures;

namespace Whydoisuck.ViewModels.CommonControlsViewModels
{
    /// <summary>
    /// View model for a datagrid about statistics of a level
    /// </summary>
    public class LevelDataGridViewModel : BaseViewModel
    {
        /// <summary>
        /// List of statistics per part that will be shown in the grid.
        /// </summary>
        public List<LevelPartStatistics> SessionStats { get; set; }
        // Statistics about a level
        private SessionsStatistics Stats { get; set; }

        public LevelDataGridViewModel(SessionsStatistics stats)
        {
            Stats = stats;
            SessionStats = FilterOutRedondantParts(stats.Statistics);
            Stats.OnStatisticsChange += Update;
        }

        // Notifies the view that there was change
        private void Update()
        {
            SessionStats = FilterOutRedondantParts(Stats.Statistics);
            OnPropertyChanged(nameof(SessionStats));
        }

        // Filters parts based on their relevance
        private List<LevelPartStatistics> FilterOutRedondantParts(List<LevelPartStatistics> stats)
        {
            var res = new List<LevelPartStatistics>();
            if (stats.Count > 0)
            {
                var prec = stats[0];
                if (prec.DeathCount > 0) res.Add(prec);//showing first element only if it brings useful information
                foreach (var part in stats.SkipWhile(t => t == stats[0]).ToList())
                {
                    if (part.DeathCount > 0 || part.ReachCount != (prec.ReachCount-prec.DeathCount))
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
