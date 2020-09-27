using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.UIModel.DataStructures;

namespace Whydoisuck.UIModel
{
    public class LevelPercentData
    {
        private const int COMPLETE_PASS_RATE = 100;

        public Range PercentRange { get; set; }
        public int ReachCount { get; set; }
        public int DeathCount { get; set; }
        public float PassRate {
            get
            {
                if (ReachCount == 0) return 0;
                if (PercentRange.Contains(100f)) return COMPLETE_PASS_RATE;
                return 100*(1-(float)DeathCount/(float)ReachCount);
            }
        }

        public int Compare(LevelPercentData p2)
        {
            int res;
            if(PercentRange.Start > p2.PercentRange.Start)
            {
                res = 1;
            } else if(PercentRange.Start < p2.PercentRange.Start)
            {
                res = -1;
            } else
            {
                res = 0;
            }
            return res;
        }
    }
}
