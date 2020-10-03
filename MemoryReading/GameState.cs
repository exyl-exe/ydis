using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.MemoryReading
{
    public class GameState
    {           
        public GDLoadedLevel LoadedLevel { get; set; }
        public GDLevelMetadata LevelMetadata { get; set; }
        public GDPlayer PlayerObject { get; set; }
    }

    public class GDLoadedLevel
    {
        public bool IsRunning { get; set; }
        public bool IsTestmode { get; set; }
        public int AttemptNumber { get; set; }
        public float PhysicalLength { get; set; }
        public float StartPosition { get; set; }
    }

    public class GDLevelMetadata
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsOnline { get; set; }
        public int OriginalID { get; set; }
        public bool IsOriginal { get; set; }
        public int Revision { get; set; }
        public bool IsCustomMusic { get; set; }
        public int MusicID { get; set; }
        public int OfficialMusicID { get; set; }
        public float MusicOffset { get; set; }
    }

    public class GDPlayer
    {
        public float XPosition { get; set; }
        public bool IsDead { get; set; }
        public bool HasWon { get; set; }
    }
}
