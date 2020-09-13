using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Utilities;

namespace Whydoisuck.DataSaving
{
    class SessionGroup
    {
        public string GroupName { get; set; }
        public string LevelName { get; set; } = "";

        public bool IsSameGroup(SessionGroup group)
        {
            return GroupName.Equals(group.GroupName);
        }

        public void AddSession(Session session)
        {
            var sessionName = GetSessionName(session);
            var path = SafePath.Combine(GetGroupDirectoryPath(), sessionName);
            session.CreateSessionFile(path);
            if (LevelName.Length == 0)
            {
                LevelName = session.Level.Name;
            }
        }

        private string GetSessionName(Session session)
        {
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
            var filePath = SafePath.Combine(groupPath, name);
            return !SafeFile.Exists(filePath);
        }

        public static string GetDefaultGroupName(Level level)
        {
            return $"{level.Name} rev{level.Revision}";
        }

        public string GetGroupDirectoryPath()
        {
            var path = SafePath.Combine(SessionSaver.SAVE_DIR, GroupName + SafePath.DirectorySeparator);
            return path;
        }

        public void CreateGroupDirectory()
        {
            SafeDirectory.CreateDirectory(GetGroupDirectoryPath());
        }
    }
}
