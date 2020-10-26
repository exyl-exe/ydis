using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Utilities;

namespace Whydoisuck.DataSaving
{
    static class GroupLoader
    {
        public static List<SessionGroup> GetAllGroups()//TODO don't deserialize sessions if not needed
        {
            var sessionManager = SerializationManager.DeserializeSessionManager();
            return sessionManager.Groups;
        }
    }
}
