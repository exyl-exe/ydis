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
        public string Number => "Attempt " + Attempt.Number;//TODO hardcoded string
        public string Percent => Attempt.EndPercent + "%";//TODO hardcoded string
        private Attempt Attempt { get; set; }

        public AttemptViewModel(Attempt a)
        {
            Attempt = a;
        }
    }
}