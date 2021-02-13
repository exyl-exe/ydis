using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydis.Model.DataStructures;
using Ydis.Model.MemoryReading.GameStateStructures;

namespace Ydis.Model.Recording
{
    public interface IRecorderState
    {
        /// <summary>
        /// Event invoked when a new session is initialized
        /// </summary>
        event Action<ISession> OnSessionInitialized;
        /// <summary>
        /// Event invoked when the current session is saved
        /// </summary>
        event Action<ISession> OnQuitSession;
        /// <summary>
        /// Event invoked when a new attempt is created
        /// </summary>
        event Action OnAttemptsUpdated;

        void OnSessionStarted(GameState state);
        void OnSessionEnded(GameState state);
        void OnAttemptStarted(GameState state);
        void OnAttemptEnded(GameState state);
    }
}
