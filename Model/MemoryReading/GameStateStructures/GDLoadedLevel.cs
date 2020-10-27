namespace Whydoisuck.Model.MemoryReading.GameStateStructures
{
    public class GDLoadedLevel
    {
        public bool IsRunning { get; set; }
        public bool IsTestmode { get; set; }
        public int AttemptNumber { get; set; }
        public float PhysicalLength { get; set; }
        public float StartPosition { get; set; }

        public GDLoadedLevel(bool isRunning, bool isTestmode, int attemptNumber, float physicalLength, float startPosition)
        {
            IsRunning = isRunning;
            IsTestmode = isTestmode;
            AttemptNumber = attemptNumber;
            PhysicalLength = physicalLength;
            StartPosition = startPosition;
        }
    }
}
