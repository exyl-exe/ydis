using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Model.MemoryReading.GameStateStructures
{
    public class GameState
    {
        public GDLoadedLevel LoadedLevel { get; set; }
        public GDLevelMetadata LevelMetadata { get; set; }
        public GDPlayer PlayerObject { get; set; }

        public GameState(GDLoadedLevel level, GDLevelMetadata metadata, GDPlayer player)
        {
            LoadedLevel = level;
            LevelMetadata = metadata;
            PlayerObject = player;
        }
    }
}
