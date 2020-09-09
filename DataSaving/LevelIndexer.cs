using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Recording;

namespace Whydoisuck.DataSaving
{
    class LevelIndexer : List<LevelEntry>//Level, filename
    {
        const int GT = 1;
        const int EQ = 0;
        const int LT = -1;

        public void SortBySimilarityTo(Level level)
        {
            Sort((key1, key2) => LevelComparison(level, key1.level, key2.level));
        }

        private static int LevelComparison(Level sample, Level key1, Level key2)
        {
            var idComp = CompareIDs(sample, key1, key2);
            if (idComp != EQ) return idComp;

            var namesComp = CompareNames(sample, key1, key2);
            if (namesComp != EQ) return namesComp;

            var originalIdComp = CompareOriginalIds(sample, key1, key2);
            if (originalIdComp != EQ) return originalIdComp;

            var musicComp = CompareMusicAndObjects(sample, key1, key2);//not separated functions because these values alone don't seem very relevant
            if (musicComp != EQ) return musicComp;

            return EQ;
        }

        private static int CompareIDs(Level sample, Level key1, Level key2)
        {
            if (!sample.IsOnline) return EQ;

            var key1Matches = (key1.ID == sample.ID);
            var key2Matches = (key2.ID == sample.ID);
            //Compare ID
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

        private static int CompareNames(Level sample, Level key1, Level key2)
        {
            var sampleName = sample.Name.ToLower();
            var key1Name = key1.Name.ToLower();
            var key2Name = key2.Name.ToLower();
            var key1Matches = key1Name.Contains(sampleName) || sampleName.Contains(key1Name);
            var key2Matches = key2Name.Contains(sampleName) || sampleName.Contains(key2Name);
            if (key1Matches && key2Matches)
            {
                var lengthComp = Math.Abs(key1.Name.Length - sample.Name.Length) - Math.Abs(key2.Name.Length - sample.Name.Length);
                if(lengthComp < 0)//oui
                {
                    return GT;
                } else if (lengthComp == 0)
                {
                    return EQ;
                } else
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

        private static int CompareOriginalIds(Level sample, Level key1, Level key2)
        {
            if (!sample.IsOnline && sample.IsOriginal) return EQ;

            var key1Matches = (key1.OriginalID == sample.ID) || (key1.OriginalID == sample.OriginalID);
            var key2Matches = (key2.OriginalID == sample.ID) || (key2.OriginalID == sample.OriginalID);
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

        private static int CompareMusicAndObjects(Level sample, Level key1, Level key2)
        {
            var key1Matches = (key1.MusicID == sample.MusicID && Math.Abs(key1.ObjectCount-sample.ObjectCount) < OBJECT_COUNT_DELTA);
            var key2Matches = (key2.MusicID == sample.MusicID && Math.Abs(key2.ObjectCount - sample.ObjectCount) < OBJECT_COUNT_DELTA);
            //Compare ID
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
    struct LevelEntry
    {
        public Level level;
        public string folderPath;
        public string fileName;
    }
}
