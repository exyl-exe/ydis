namespace Whydoisuck.Model.MemoryReading.GameStateStructures
{
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

        public GDLevelMetadata(int iD, string name, bool isOnline, int originalID, bool isOriginal, int revision, bool isCustomMusic, int musicID, int officialMusicID, float musicOffset)
        {
            ID = iD;
            Name = name;
            IsOnline = isOnline;
            OriginalID = originalID;
            IsOriginal = isOriginal;
            Revision = revision;
            IsCustomMusic = isCustomMusic;
            MusicID = musicID;
            OfficialMusicID = officialMusicID;
            MusicOffset = musicOffset;
        }
    }
}
