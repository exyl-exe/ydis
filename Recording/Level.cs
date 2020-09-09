using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Recording
{
    class Level
    {
        public const int OBJECT_COUNT_DELTA = 10;//Just in case some objects are deleted accidentally on copy
        public int ID { get; set; }
        public bool IsOnline { get; set; }
        public int OriginalID { get; set; }
        public bool IsOriginal { get; set; }
        public string Name { get; set; }
        public int Revision { get; set; }
        public int MusicID { get; set; }
        public int ObjectCount { get; set; }

        public bool IsSameLevel(Level l)
        {
            return
                ID == l.ID &&
                IsOnline == l.IsOnline &&
                OriginalID == l.OriginalID &&
                Name.Equals(l.Name) &&
                Revision == l.Revision &&
                MusicID == l.MusicID &&
                ObjectCount == l.ObjectCount;
        }

        public bool CanBeSameLevel(Level l)//TODO return level of similarity instead of bool, and use this in levelindexer ?
        {
            var test =
                (ID == l.ID) ||
                (OriginalID == l.OriginalID || OriginalID == l.ID || ID == l.OriginalID) ||
                (Name.ToLower().Contains(l.Name.ToLower()) || l.Name.ToLower().Contains(Name.ToLower())) ||
                (MusicID == l.MusicID && (Math.Abs(ObjectCount - l.ObjectCount)<OBJECT_COUNT_DELTA));
            return test;
        }
    }
}
