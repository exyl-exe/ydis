using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.ViewModels.CommonControlsViewModels
{
    public class PracticeAttemptViewModel
    {
        public int Number => Attempt.Number;
        public float StartPercent => Attempt.StartPercent;
        public float EndPercent => Attempt.EndPercent;
        public float RunLength => Attempt.EndPercent - Attempt.StartPercent;
        private PracticeAttempt Attempt { get; }

        public PracticeAttemptViewModel(PracticeAttempt practiceAttempt)
        {
            Attempt = practiceAttempt;
        }
    }
}