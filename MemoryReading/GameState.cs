using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.MemoryReading
{
    class GameState
    {           
        public GDLoadedLevelInfos PlayedLevel { get; set; }
        public GDPlayerInfos PlayerObject { get; set; }       
    }

    class GDLoadedLevelInfos
    {
        public int ID { get; set; }
        public string Name { get; set; }
        //public float StartPosition { get { throw new NotImplementedException(); } } TODO couldn't find how to get it from memory
        public int AttemptNumber { get; set; }
        public float PhysicalLength { get; set; }
        public bool IsOnline { get; set; }
        public int OriginalID { get; set; }
        public bool IsOriginal { get; set; }
        public int Revision { get; set; }
        public bool IsCustomMusic { get; set; }
        public int MusicID { get; set; }
        public int OfficialMusicID { get; set; }
        public float MusicOffset { get; set; }
    }

    class GDPlayerInfos
    {
        public float XPosition { get; set; }
        public bool IsDead { get; set; }
        public bool HasWon { get; set; }
    }
}
