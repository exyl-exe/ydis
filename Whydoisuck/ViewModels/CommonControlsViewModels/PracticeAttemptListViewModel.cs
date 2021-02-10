using System.Linq;
using System.Collections.Generic;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.ViewModels.CommonControlsViewModels
{
    /// <summary>
    /// View model for practice attempt lists
    /// </summary>
    public class PracticeAttemptListViewModel : BaseViewModel
    {
        public List<PracticeAttemptViewModel> Attempts { get; private set; }

        public PracticeAttemptListViewModel(List<PracticeAttempt> attempts)
        {
            Attempts = attempts.Select(a => new PracticeAttemptViewModel(a)).ToList(); ;
        }
    }
}