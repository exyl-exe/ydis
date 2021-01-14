using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.ViewModels.DataViewModels
{
    /// <summary>
    /// View model for one attempt
    /// </summary>
    public class AttemptViewModel : BaseViewModel
    {
        /// <summary>
        /// How the attempt number should be displayed
        /// </summary>
        public string Number => string.Format(Properties.Resources.AttemptListNumberFormat, Attempt.Number);
        /// <summary>
        /// How the attempt end percent should be displayed
        /// </summary>
        public string Percent => string.Format(Properties.Resources.AttemptListPercentFormat, Attempt.EndPercent);
        // Attempt to display
        private Attempt Attempt { get; set; }

        public AttemptViewModel(Attempt a)
        {
            Attempt = a;
        }
    }
}