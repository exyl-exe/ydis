namespace Whydoisuck.Model.MemoryReading.GameStateStructures
{
    public class GDPlayer
    {
        public float XPosition { get; set; }
        public bool IsDead { get; set; }
        public bool HasWon { get; set; }

        public GDPlayer(float xPosition, bool isDead, bool hasWon)
        {
            XPosition = xPosition;
            IsDead = isDead;
            HasWon = hasWon;
        }
    }
}
