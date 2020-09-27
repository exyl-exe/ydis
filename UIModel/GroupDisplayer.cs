using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;

namespace Whydoisuck.UIModel
{
    public class GroupDisplayer
    {
        public List<Session> GroupSessions { get; set; }//TODO shouldn't be loaded directly
        public string GroupName { get; set; }
    }
}
