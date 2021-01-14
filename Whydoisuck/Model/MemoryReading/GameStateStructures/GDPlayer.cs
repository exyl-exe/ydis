namespace Whydoisuck.Model.MemoryReading.GameStateStructures
{
    /// <summary>
    /// Class <c>GDPlayer</c> represents a player object in the game
    /// </summary>
    public class GDPlayer
    {
        /// <summary>
        /// Position on the X-axis of the player
        /// </summary>
        public float XPosition { get; set; }
        /// <summary>
        /// Boolean true if the player has died
        /// </summary>
        public bool IsDead { get; set; }
        /// <summary>
        /// Boolean true if the player has reached the end of the level
        /// </summary>
        public bool HasWon { get; set; }

        public GDPlayer(float xPosition, bool isDead, bool hasWon)
        {
            XPosition = xPosition;
            IsDead = isDead;
            HasWon = hasWon;
        }
    }
}
