namespace Ydis.Model.MemoryReading.GameStateStructures
{
    /// <summary>
    /// Class <c>GDLevelMetadata</c> represents a level's metadata in the game.
    /// </summary>
    public class GDLevelMetadata
    {
        /// <summary>
        /// Online ID of the level
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Name of the level
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// True if the level is uploaded
        /// </summary>
        public bool IsOnline { get; set; }
        /// <summary>
        /// ID of the original, if the level is a copy
        /// </summary>
        public int OriginalID { get; set; }
        /// <summary>
        /// True if the level isn't copied
        /// </summary>
        public bool IsOriginal { get; set; }
        /// <summary>
        /// Revision of the level
        /// </summary>
        public int Revision { get; set; }
        /// <summary>
        /// True if the level uses a music from newgrounds
        /// </summary>
        public bool IsCustomMusic { get; set; }
        /// <summary>
        /// ID of the used music on newgrounds
        /// </summary>
        public int MusicID { get; set; }
        /// <summary>
        /// ID of the music, if the music is from the official levels
        /// </summary>
        public int OfficialMusicID { get; set; }
        /// <summary>
        /// Offset in seconds of the level's music
        /// </summary>
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
