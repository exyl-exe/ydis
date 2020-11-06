namespace Whydoisuck.Model.MemoryReading.GameStateStructures
{
    /// <summary>
    /// Class <c>GDLoadedLevel</c> represents the level object in the game
    /// </summary>
    public class GDLoadedLevel
    {
        /// <summary>
        /// Boolean true if the level is actually on going, and not loading
        /// </summary>
        public bool IsRunning { get; set; }
        /// <summary>
        /// Boolean true if the current session is on a copy
        /// </summary>
        public bool IsTestmode { get; set; }
        /// <summary>
        /// Current attempt
        /// </summary>
        public int AttemptNumber { get; set; }
        /// <summary>
        /// Length of the level, used to compute current %
        /// </summary>
        public float PhysicalLength { get; set; }
        /// <summary>
        /// Position on the X-axis at which the player object respawns
        /// </summary>
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
