using System.Collections.Generic;
using System.Linq;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.DataViewModels;

namespace Whydoisuck.ViewModels.CommonControlsViewModels
{
    /// <summary>
    /// View model for a simple list of attempt
    /// </summary>
    public class AttemptListViewModel : BaseViewModel
    {
        /// <summary>
        /// Attempts in the list
        /// </summary>
        public List<AttemptViewModel> Attempts { get; set; }

        public AttemptListViewModel(List<Attempt> attempts)
        {
            Attempts = attempts.Select(a => new AttemptViewModel(a)).ToList();
        }
    }
}