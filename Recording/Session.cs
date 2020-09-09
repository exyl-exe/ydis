using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Recording
{
    class Session
    {
        public Level Level { get; set; }
        public DateTime StartTime { get; set; }
        public List<Attempt> Attempts { get; set; }

        public void AddAttempt(Attempt att)
        {
            Attempts.Add(att);
        }
    }
}
