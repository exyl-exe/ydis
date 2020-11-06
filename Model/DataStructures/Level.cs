using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Whydoisuck.Model.MemoryReading.GameStateStructures;

namespace Whydoisuck.Model.DataStructures
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

        //Constants for comparisons
        const int GT = 1;
        const int EQ = 0;
        const int LT = -1;

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
        public bool CouldBeSameLevel(Level level)
        {
            if (level == null) return false;
            if (IsOnline && level.IsOnline)
            {
                return ID == level.ID;
            }

            if (FromSameLevel(level))
            {
                return true;
            }

            if (SimilarName(level))
            {
                return true;
            }

            if (SameMusic(level) && SamePhysicalLength(level))
            {
                return true;
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
            var idComp = CompareIDs(sample, level1, level2);
            if (idComp != EQ) return idComp;

            var namesComp = CompareNames(sample, level1, level2);
            if (namesComp != EQ) return namesComp;

            var originalIdComp = CompareOriginalIds(sample, level1, level2);
            if (originalIdComp != EQ) return originalIdComp;

            var musicComp = CompareMusicAndLength(sample, level1, level2);//not separated functions because these values alone don't seem very relevant
            if (musicComp != EQ) return musicComp;

            return EQ;
        }

        // Returns true if two levels are copyables from the same level.
        private bool FromSameLevel(Level level)
        {
            if (level == null) return false;

            if (IsOriginal && !level.IsOriginal)
            {
                if (ID == level.OriginalID)
                {
                    return true;
                }
            }

            if (!IsOriginal && level.IsOriginal)
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

            return ID != 0 && ID == level.ID;
        }

        // Returns true if two levels have a similar name
        private bool SimilarName(Level level)
        {
            if (level == null) return false;
            return Name.ToLower().Contains(level.Name.ToLower()) || level.Name.ToLower().Contains(Name.ToLower());
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

        // Returns true if two levels have the same length
        private bool SamePhysicalLength(Level level)
        {
            if (level == null) return false;
            return Math.Abs(PhysicalLength - level.PhysicalLength) <= LENGTH_EPSILON;
        }

        // Compares the IDs of two levels relative to a sample.
        private static int CompareIDs(Level sample, Level level1, Level level2)
        {
            if (!sample.IsOnline) return EQ;

            var key1Matches = level1.ID == sample.ID;
            var key2Matches = level2.ID == sample.ID;
            if (key1Matches && key2Matches)
            {
                return EQ;
            }
            else if (key1Matches)
            {
                return GT;
            }
            else if (key2Matches)
            {
                return LT;
            }
            return EQ;
        }

        // Compares the names of two levels relative to a sample.
        private static int CompareNames(Level sample, Level level1, Level level2)
        {
            var sampleName = sample.Name.ToLower();
            var key1Name = level1.Name.ToLower();
            var key2Name = level2.Name.ToLower();
            var key1Matches = key1Name.Contains(sampleName) || sampleName.Contains(key1Name);
            var key2Matches = key2Name.Contains(sampleName) || sampleName.Contains(key2Name);
            if (key1Matches && key2Matches)
            {
                var lengthComp = Math.Abs(level1.Name.Length - sample.Name.Length) - Math.Abs(level2.Name.Length - sample.Name.Length);
                if (lengthComp < 0)//oui
                {
                    return GT;
                }
                else if (lengthComp == 0)
                {
                    return EQ;
                }
                else
                {
                    return LT;
                }
            }
            else if (key1Matches)
            {
                return GT;
            }
            else if (key2Matches)
            {
                return LT;
            }
            return EQ;
        }

        // Compares the original IDs of two levels relative to a sample.
        private static int CompareOriginalIds(Level sample, Level level1, Level level2)
        {
            if (!sample.IsOnline && sample.IsOriginal) return EQ;//if the sample is not uploaded and is original, there is no need to check for original IDs of compared levels

            var key1Matches = level1.FromSameLevel(sample);
            var key2Matches = level2.FromSameLevel(sample);
            if (key1Matches && key2Matches)
            {
                return EQ;
            }
            else if (key1Matches)
            {
                return GT;
            }
            else if (key2Matches)
            {
                return LT;
            }
            return EQ;
        }

        // Compares the musics of two levels relative to a sample.
        private static int CompareMusicAndLength(Level sample, Level level1, Level level2)
        {
            var key1Matches = level1.SameMusic(sample) && level1.SamePhysicalLength(sample);
            var key2Matches = level2.SameMusic(sample) && level2.SamePhysicalLength(sample);
            if (key1Matches && key2Matches)
            {
                return EQ;
            }
            else if (key1Matches)
            {
                return GT;
            }
            else if (key2Matches)
            {
                return LT;
            }
            return EQ;
        }
    }
}
