using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;

namespace Whydoisuck.UIModel
{
    class SessionFilter
    {
        public bool ShowNormalRuns { get; set; }
        public bool ShowCopyRuns { get; set; }

        public SessionFilter(bool showNormal, bool showCopy)
        {
            ShowNormalRuns = showNormal;
            ShowCopyRuns = showCopy;
        }

        public bool Matches(Session s)
        {
            if (s.IsCopyRun)
            {
                return ShowCopyRuns;
            } else
            {
                return ShowNormalRuns;
            }
        }
    }
}
