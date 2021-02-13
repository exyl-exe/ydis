using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ydis.Model.MemoryReading.GameStateStructures;

namespace Ydis.Model.DataStructures
{
    /// <summary>
    /// Represent an individual level in the game. A level and it's copy
    /// are separated individual levels.
    /// </summary>
    public class Level
    {
        /// <summary>
        /// Epsilon for level length comparisons
        /// </summary>
        public const float LENGTH_EPSILON = 1f;
        /// <summary>
        /// Epsilon for music offset comparisons
        /// </summary>
        public const float MUSIC_OFFSET_EPSILON = 0.1f;//seconds

        /// <summary>
        /// Online ID of the level
        /// </summary>
        [JsonProperty(PropertyName = "ID")] public int ID { get; set; }
        /// <summary>
        /// True if the level is uploaded
        /// </summary>
        [JsonProperty(PropertyName = "IsOnline")] public bool IsOnline { get; set; }
        /// <summary>
        /// ID of the original, if the level is a copy
        /// </summary>
        [JsonProperty(PropertyName = "OriginalID")] public int OriginalID { get; set; }
        /// <summary>
        /// True if the level isn't copied
        /// </summary>
        [JsonProperty(PropertyName = "IsOriginal")] public bool IsOriginal { get; set; }
        /// <summary>
        /// Name of the level
        /// </summary>
        [JsonProperty(PropertyName = "Name")] public string Name { get; set; }
        /// <summary>
        /// Revision of the level
        /// </summary>
        [JsonProperty(PropertyName = "Revision")] public int Revision { get; set; }
        /// <summary>
        /// Length of the level on the X-axis
        /// </summary>
        [JsonProperty(PropertyName = "PhysicalLength")] public float PhysicalLength { get; set; }
        /// <summary>
        /// True if the level uses a music from newgrounds
        /// </summary>
        [JsonProperty(PropertyName = "IsCustomMusic")] public bool IsCustomMusic { get; set; }
        /// <summary>
        /// ID of the used music on newgrounds
        /// </summary>
        [JsonProperty(PropertyName = "MusicID")] public int MusicID { get; set; }
        /// <summary>
        /// ID of the music, if the music is from the official levels
        /// </summary>
        [JsonProperty(PropertyName = "OfficialMusicID")] public int OfficialMusicID { get; set; }
        /// <summary>
        /// Offset in seconds of the level's music
        /// </summary>
        [JsonProperty(PropertyName = "MusicOffset")] public float MusicOffset { get; set; }

        public Level() { }//for json deserializer

        public Level(GameState state)
        {
            if (state != null && state.LevelMetadata != null & state.LoadedLevel != null)
            {
                ID = state.LevelMetadata.ID;
                IsOnline = state.LevelMetadata.IsOnline;
                OriginalID = state.LevelMetadata.OriginalID;
                IsOriginal = state.LevelMetadata.IsOriginal;
                Name = state.LevelMetadata.Name;
                Revision = state.LevelMetadata.Revision;
                PhysicalLength = state.LoadedLevel.PhysicalLength;
                IsCustomMusic = state.LevelMetadata.IsCustomMusic;
                MusicID = state.LevelMetadata.MusicID;
                OfficialMusicID = state.LevelMetadata.OfficialMusicID;
                MusicOffset = state.LevelMetadata.MusicOffset;
            }
        }

        /// <summary>
        /// Checks if this level is identical to another individual level
        /// </summary>
        /// <param name="level">The level to compare this instance to</param>
        /// <returns>True if this level is the same as the given level</returns>
        public bool IsSameLevel(Level level)
        {
            if (level == null)
            {
                return false;
            }
            else
            {
                return
                ID == level.ID &&
                IsOnline == level.IsOnline &&
                OriginalID == level.OriginalID &&
                Name.Equals(level.Name) &&
                Revision == level.Revision &&
                SameMusic(level) &&
                SamePhysicalLength(level);
            }
        }

        /// <summary>
        /// Checks if this level could be in the same group as another level.
        /// </summary>
        /// <param name="level">A level to compare this level to</param>
        /// <returns>True if the two levels would belong to the same group</returns>
        public bool ShouldBeGroupedWith(Level level)
        {
            if (level == null) return false;

            // If at least one is a copy, check if originalID/IDs match
            // Also check for length to put in different groups modified copies
            if(!IsOriginal || !level.IsOriginal)
            {
                return SamePhysicalLength(level) && FromSameLevel(level);
            }

            // If both are original levels, they won't be in the same group unless :
            // They are actually just one level, which is uploaded
            if(IsOnline && level.IsOnline)
            {
                return ID == level.ID;
            }

            // One is a copy of the other, which isn't uploaded
            // To check for copies of unuploaded level, length and music are compared
            if(SamePhysicalLength(level) && SameMusic(level))
            {
                return true;
            }

            // If none of the condition matched then they are not in the same group
            return false;
        }

        // Returns true if two levels have the same length
        private bool SamePhysicalLength(Level level)
        {
            if (level == null) return false;
            return Math.Abs(PhysicalLength - level.PhysicalLength) <= LENGTH_EPSILON;
        }


        // Returns true if two levels are copies of the same level.
        private bool FromSameLevel(Level level)
        {
            if (level == null) return false;

            if(IsOriginal && IsOnline && level.IsOriginal && level.IsOnline)
            {
                return ID == level.ID;
            }

            if (IsOriginal && IsOnline && !level.IsOriginal)
            {
                if (ID == level.OriginalID)
                {
                    return true;
                }
            }

            if (!IsOriginal && level.IsOriginal && level.IsOnline)
            {
                if (OriginalID == level.ID)
                {
                    return true;
                }
            }

            if (!IsOriginal && !level.IsOriginal)
            {
                return OriginalID == level.OriginalID;
            }

            return false;
        }

        // Returns true if two levels use the same music
        private bool SameMusic(Level level)
        {
            if (level == null) return false;
            if (!(Math.Abs(MusicOffset - level.MusicOffset) < MUSIC_OFFSET_EPSILON))
            {
                return false;
            }

            if (level.IsCustomMusic && IsCustomMusic)
            {
                if (level.MusicID == MusicID)
                {
                    return true;
                }
            }

            if (!level.IsCustomMusic && !IsCustomMusic)
            {
                if (level.OfficialMusicID == OfficialMusicID)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Compares two levels to a sample.
        /// </summary>
        /// <param name="sample">The level to compare the levels to</param>
        /// <param name="level1">One level</param>
        /// <param name="level2">Another level</param>
        /// <returns>1 if the first level is the most similar
        /// -1 if the second level is the most similar
        /// 0 if both levels are equally similar</returns>
        public static int CompareToSample(Level sample, Level level1, Level level2)
        {
            var id = CompareIDs(sample, level1, level2);
            var origin = CompareOrigin(sample, level1, level2);
            var length = CompareLength(sample, level1, level1);
            var music = CompareMusic(sample, level1, level2);
            var names = CompareNames(sample, level1, level2);
            if (id != 0) return id;
            if (origin != 0 && origin == length) return origin; //Same original level and same length
            if (origin != 0) return origin;
            if (length != 0) return length;
            if (music != 0) return music;
            if (names != 0) return names;
            return 0;
        }

        // Compares the original IDs and IDs of two levels relative to a sample.
        // Only matches if the id is identical
        private static int CompareIDs(Level sample, Level level1, Level level2)
        {
            bool key1Matches, key2Matches;
            if (sample.IsOnline)
            {
                key1Matches = level1.IsOnline && level1.ID == sample.ID;
                key2Matches = level2.IsOnline && level2.ID == sample.ID;
            } else
            {
                // Needed so copies can't match an online level
                key1Matches = !level1.IsOnline;
                key2Matches = !level2.IsOnline;
            }
            var res = 0;
            if (key1Matches) res += 1;
            if (key2Matches) res -= 1;
            return res;
        }

        // Compares the original IDs and IDs of two levels relative to a sample.
        private static int CompareOrigin(Level sample, Level level1, Level level2)
        {
            var key1Matches = !level1.IsOriginal && !sample.IsOriginal && level1.OriginalID == sample.OriginalID;
            var key2Matches = !level2.IsOriginal && !sample.IsOriginal && level2.OriginalID == sample.OriginalID;
            var res = 0;
            if (key1Matches) res += 1;
            if (key2Matches) res -= 1;
            return res;
        }

        // Compares the names of two levels relative to a sample.
        private static int CompareNames(Level sample, Level level1, Level level2)
        {
            var sampleName = sample.Name.ToLower();
            var key1Name = level1.Name.ToLower();
            var key2Name = level2.Name.ToLower();
            var key1Matches = key1Name.Contains(sampleName) || sampleName.Contains(key1Name);
            var key2Matches = key2Name.Contains(sampleName) || sampleName.Contains(key2Name);
            var res = 0;
            if (key1Matches) res += 1;
            if (key2Matches) res -= 1;
            return res;
        }

        // Compares the musics of two levels relative to a sample.
        private static int CompareMusic(Level sample, Level level1, Level level2)
        {
            var key1Matches = level1.SameMusic(sample);
            var key2Matches = level2.SameMusic(sample);
            var res = 0;
            if (key1Matches) res += 1;
            if (key2Matches) res -= 1;
            return res;
        }

        // Compares the lengths of two levels relative to a sample.
        private static int CompareLength(Level sample, Level level1, Level level2)
        {
            var key1Matches = level1.SamePhysicalLength(sample);
            var key2Matches = level2.SamePhysicalLength(sample);
            var res = 0;
            if (key1Matches) res += 1;
            if (key2Matches) res -= 1;
            return res;
        }
    }
}
