using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Model.MemoryReading.GameStateStructures
{
    /// <summary>
    /// Class <c>GameState</c> represents the state of the game at one point
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Contains information about the currently played level
        /// </summary>
        public GDLoadedLevel LoadedLevel { get; set; }
        /// <summary>
        /// Contains information about the metadata of the currently played level
        /// </summary>
        public GDLevelMetadata LevelMetadata { get; set; }
        /// <summary>
        /// Contains information about the player object in the game
        /// </summary>
        public GDPlayer PlayerObject { get; set; }

        public GameState(GDLoadedLevel level, GDLevelMetadata metadata, GDPlayer player)
        {
            LoadedLevel = level;
            LevelMetadata = metadata;
            PlayerObject = player;
        }
    }
}
