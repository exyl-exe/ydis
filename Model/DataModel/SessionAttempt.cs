using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.DataModel
{
    public class SessionAttempt
    {
        public Session Session { get; set; }
        public Attempt Attempt { get; set; }

        public SessionAttempt(Session session, Attempt attempt)
        {
            Session = session;
            Attempt = attempt;
        }
    }
}
