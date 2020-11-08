using System.Collections.Generic;
using System.Linq;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.ViewModels.CommonControlsViewModels
{
    public class AttemptListViewModel : BaseViewModel
    {
        public List<AttemptViewModel> Attempts { get; set; }

        public AttemptListViewModel(List<Attempt> attempts)
        {
            Attempts = attempts.Select(a => new AttemptViewModel(a)).ToList();
        }
    }

    public class AttemptViewModel : BaseViewModel
    {
        public string Number => string.Format(Properties.Resources.AttemptListNumberFormat, Attempt.Number);
        public string Percent => string.Format(Properties.Resources.AttemptListPercentFormat, Attempt.EndPercent);
        private Attempt Attempt { get; set; }

        public AttemptViewModel(Attempt a)
        {
            Attempt = a;
        }
    }
}