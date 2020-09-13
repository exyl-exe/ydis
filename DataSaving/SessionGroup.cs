using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.DataSaving
{
    class SessionGroup
    {
        public string Name { get; set; }

        public bool IsSameGroup(SessionGroup group)
        {
            return Name.Equals(group.Name);
        }

        public void AddSession(Session session)
        {
            var sessionName = GetSessionName(session);
            var path = Path.Combine(GetGroupDirectoryPath(), sessionName);
            session.CreateSessionFile(path);
        }

        private string GetSessionName(Session session)
        {
            var dir = GetGroupDirectoryPath();
            var defaultName = session.GetDefaultSessionFileName();
            var name = defaultName;
            var i = 2;
            while (!IsSessionNameAvailable(name))
            {
                name = $"{defaultName} ({i})";
                i++;
            }
            return name;
        }

        private bool IsSessionNameAvailable(string name)
        {
            var groupPath = GetGroupDirectoryPath();
            var filePath = Path.Combine(groupPath, name);
            return !File.Exists(filePath);
        }

        public static string GetDefaultGroupName(Level level)
        {
            return $"{level.Name} rev{level.Revision}";
        }

        public string GetGroupDirectoryPath()
        {
            return Path.Combine(SessionSaver.SAVE_DIR, Name + "\\");
        }

        public void CreateGroupDirectory()
        {
            Directory.CreateDirectory(GetGroupDirectoryPath());
        }
    }
}
