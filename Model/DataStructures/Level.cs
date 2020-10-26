using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Whydoisuck.MemoryReading;

namespace Whydoisuck.Model.DataStructures
{
    public class Level
    {
        const int GT = 1;
        const int EQ = 0;
        const int LT = -1;

        public const float LENGTH_EPSILON = 1f;//game space unit
        public const float MUSIC_OFFSET_EPSILON = 0.1f;//seconds

        [JsonProperty(PropertyName = "ID")] public int ID { get; set; }
        [JsonProperty(PropertyName = "IsOnline")] public bool IsOnline { get; set; }
        [JsonProperty(PropertyName = "OriginalID")] public int OriginalID { get; set; }
        [JsonProperty(PropertyName = "IsOriginal")] public bool IsOriginal { get; set; }
        [JsonProperty(PropertyName = "Name")] public string Name { get; set; }
        [JsonProperty(PropertyName = "Revision")] public int Revision { get; set; }
        [JsonProperty(PropertyName = "PhysicalLength")] public float PhysicalLength { get; set; }
        [JsonProperty(PropertyName = "IsCustomMusic")] public bool IsCustomMusic { get; set; }
        [JsonProperty(PropertyName = "MusicID")] public int MusicID { get; set; }
        [JsonProperty(PropertyName = "OfficialMusicID")] public int OfficialMusicID { get; set; }
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

        public bool FromSameLevel(Level level)
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
                //If a not original level is copied, then the original ID of the copy is the same as the original ID of the copied level
                //Therefore there is no need to test if the original ID of one level is the online ID of the other if both levels aren't original
                return OriginalID == level.OriginalID;
            }

            return ID != 0 && ID == level.ID;//Effectively always false, but it ensures an uploaded level is from the same level as itself
        }

        public bool SimilarName(Level level)
        {
            if (level == null) return false;
            return Name.ToLower().Contains(level.Name.ToLower()) || level.Name.ToLower().Contains(Name.ToLower());
        }

        public bool SameMusic(Level level)
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

        public bool SamePhysicalLength(Level level)
        {
            if (level == null) return false;
            return Math.Abs(PhysicalLength - level.PhysicalLength) <= LENGTH_EPSILON;
        }

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
