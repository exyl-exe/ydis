using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.DataSaving
{
    class Level
    {
        const int GT = 1;//TODO might not be necessary
        const int EQ = 0;
        const int LT = -1;

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

        public static int LevelComparison(Level sample, Level level1, Level level2)
        {
            var idComp = CompareIDs(sample, level1, level2);
            if (idComp != EQ) return idComp;

            var namesComp = CompareNames(sample, level1, level2);
            if (namesComp != EQ) return namesComp;

            var originalIdComp = CompareOriginalIds(sample, level1, level2);
            if (originalIdComp != EQ) return originalIdComp;

            var musicComp = CompareMusicAndObjects(sample, level1, level2);//not separated functions because these values alone don't seem very relevant
            if (musicComp != EQ) return musicComp;

            return EQ;
        }

        private static int CompareIDs(Level sample, Level level1, Level level2)
        {
            if (!sample.IsOnline) return EQ;

            var key1Matches = (level1.ID == sample.ID);
            var key2Matches = (level2.ID == sample.ID);
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
            if (!sample.IsOnline && sample.IsOriginal) return EQ;

            var key1Matches = (level1.OriginalID == sample.ID) || (level1.OriginalID == sample.OriginalID);
            var key2Matches = (level2.OriginalID == sample.ID) || (level2.OriginalID == sample.OriginalID);
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

        private static int CompareMusicAndObjects(Level sample, Level level1, Level level2)
        {
            var key1Matches = (level1.MusicID == sample.MusicID && Math.Abs(level1.ObjectCount - sample.ObjectCount) < Level.OBJECT_COUNT_DELTA);
            var key2Matches = (level2.MusicID == sample.MusicID && Math.Abs(level2.ObjectCount - sample.ObjectCount) < Level.OBJECT_COUNT_DELTA);
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
}
