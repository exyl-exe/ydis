using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Model.DataStructures
{
    /// <summary>
    /// Data of a folder
    /// </summary>
    public class SessionGroupData
    {
        /// <summary>
        /// Sessions of the group
        /// </summary>
        public List<Session> Sessions { get; set; }

        // Creates the data for a group containing a single session
        public SessionGroupData(Session s)
        {
            Sessions = new List<Session>() { s };
        }

        public SessionGroupData(List<Session> s)
        {
            Sessions = s;
        }

        /// <summary>
        /// Merges the data of two groups
        /// </summary>
        public void Merge(SessionGroupData groupData)
        {
            Sessions.AddRange(groupData.Sessions);
        }

        public void AddSession(Session session)
        {
            Sessions.Add(session);
        }
    }
}
