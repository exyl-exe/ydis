using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.MemoryReading.GameStateStructures;

namespace Whydoisuck.Model.Recording
{
    public interface IRecorderState
    {
        // Creates a new session for the given level
        void CreateNewSession(GDLevelMetadata level);
        // Updates session values once the level is fully loaded
        void UpdateSessionOnLoad(GameState state);
        // Saves the current session and stops managing it
        void PopSaveCurrentSession(GameState state);
        // Create a new attempt for the current session
        void CreateNewAttempt(GameState state);
        // Saves the current (losing) attempt and stops managing it
        void PopSaveLosingAttempt(GameState state);
        // Saves the current (winning) attempt and stops managing it
        void PopSaveWinningAttempt(GameState state);
    }
}
