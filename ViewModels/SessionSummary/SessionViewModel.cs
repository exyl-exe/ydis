using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;

namespace Whydoisuck.ViewModels.SessionSummary
{
    public class SessionViewModel : BaseViewModel
    {
        private Session Session { get; set; }
        public SessionViewModel(Session s)
        {
            Session = s;
        }
    }
}
