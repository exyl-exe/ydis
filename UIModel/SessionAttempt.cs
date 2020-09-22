using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;

namespace Whydoisuck.UIModel
{
    struct SessionAttempt
    {
        public Session Session {get;set;}
        public Attempt Attempt { get; set; }
    }
}
