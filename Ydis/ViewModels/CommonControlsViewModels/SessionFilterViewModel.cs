using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydis.Model.DataStructures;

namespace Ydis.ViewModels.CommonControlsViewModels
{
    /// <summary>
    /// Model for the session filtering panel
    /// </summary>
    public class SessionFilterViewModel : BaseViewModel
    {
        private bool _showNormal;
        /// <summary>
        /// Wether sessions starting from 0% should be shown or hidden.
        /// </summary>
        public bool ShowNormal
        {
            get
            {
                return _showNormal;
            }
            set
            {
                _showNormal = value;
                OnFilterChanges?.Invoke();
            }
        }

        private bool _showCopy;
        /// <summary>
        /// Wether sessions using a start position should be shown or hidden.
        /// </summary>
        public bool ShowCopy
        {
            get
            {
                return _showCopy;
            }
            set
            {
                _showCopy = value;
                OnFilterChanges?.Invoke();
            }
        }

        private bool _showPractice;
        /// <summary>
        /// Wether sessions using a start position should be shown or hidden.
        /// </summary>
        public bool ShowPractice
        {
            get
            {
                return _showPractice;
            }
            set
            {
                _showPractice = value;
                OnFilterChanges?.Invoke();
            }
        }

        /// <summary>
        /// Delegate for callbacks when the criterias change.
        /// </summary>
        public delegate void OnFilterChangesCallback();
        /// <summary>
        /// Invoked when the filtering criterias are updated.
        /// </summary>
        public event OnFilterChangesCallback OnFilterChanges;

        public SessionFilterViewModel()
        {
            ShowNormal = true;
            ShowCopy = true;
            ShowPractice = true;
        }

        /// <summary>
        /// Checks if a session matches the current filter.
        /// </summary>
        /// <param name="s">The session to check.</param>
        /// <returns>True if the session should be shown, false if it should be hidden.</returns>
        public bool Matches(Session s)
        {
            return (s.IsCopyRun && ShowCopy) || (!s.IsCopyRun && ShowNormal);
        }
    }
}
