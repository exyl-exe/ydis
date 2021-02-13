using System.Collections.Generic;
using System.Linq;
using Ydis.Model.DataStructures;
using Ydis.ViewModels.DataViewModels;

namespace Ydis.ViewModels.CommonControlsViewModels
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