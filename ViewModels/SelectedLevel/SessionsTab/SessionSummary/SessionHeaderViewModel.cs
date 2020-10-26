using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.ViewModels.SessionSummary
{
    public class SessionHeaderViewModel : BaseViewModel
    {
        public Session Session { get; set; }

        public SessionHeaderViewModel(Session s)
        {
            Session = s;
        }
    }
}
