using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.UIModel
{
    class LevelPercentData
    { 
        public Range PercentRange { get; set; }
        public int ReachCount { get; set; }
        public int DeathCount { get; set; }
        public float PassRate {
            get
            {
                if (ReachCount == 0) return 0;
                return 100*(1-(float)DeathCount/(float)ReachCount);
            }
        }
    }
}
