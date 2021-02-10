using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.ViewModels.DataViewModels
{
    /// <summary>
    /// View model for one attempt
    /// </summary>
    public class AttemptViewModel : BaseViewModel
    {
        public int Number => Attempt.Number;
        public float Percent => Attempt.EndPercent;
        // Attempt to display
        private Attempt Attempt { get; set; }

        public AttemptViewModel(Attempt a)
        {
            Attempt = a;
        }
    }
}