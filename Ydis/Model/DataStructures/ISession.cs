using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ydis.Model.DataStructures
{
    public interface ISession
    {
        /// <summary>
        /// Level the session was played on
        /// </summary>
        Level Level { get; set; }
        /// <summary>
        /// True if the session was played from a startpos.
        /// </summary>
        bool IsCopyRun { get; set; }
        /// <summary>
        /// Time the session was started at
        /// </summary>
        DateTime StartTime { get; set; }
        /// <summary>
        /// How long was the session
        /// </summary>
        TimeSpan Duration { get; set; }
    }
}
