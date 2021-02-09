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
    public class SessionGroupData : WDISSerializable
    {
        /// <summary>
        /// Sessions of the group
        /// </summary>
        [JsonProperty(PropertyName = "Sessions")] public List<Session> Sessions { get; set; }
        [JsonProperty(PropertyName = "PracticeSessions")] public List<PracticeSession> PracticeSessions { get; set; }


        public SessionGroupData():this(new List<Session>(), new List<PracticeSession>())
        { }

        // Creates the data for a group containing a single session
        public SessionGroupData(Session s) : this(new List<Session>() { s }, new List<PracticeSession>())
        { }

        public SessionGroupData(List<Session> s, List<PracticeSession> ps)
        {
            Sessions = s;
            PracticeSessions = ps;
        }

        /// <summary>
        /// Merges the data of two groups
        /// </summary>
        public void Merge(SessionGroupData groupData)
        {
            Sessions.AddRange(groupData.Sessions);
            PracticeSessions.AddRange(groupData.PracticeSessions);
        }

        //Adds a normal session to the group
        public void AddSession(Session session)
        {
            Sessions.Add(session);
        }

        //Adds a practice session to the group
        public void AddPracticeSession(PracticeSession practiceSession)
        {
            PracticeSessions.Add(practiceSession);
        }
    }
}
