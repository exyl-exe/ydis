using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ydis.ViewModels.CommonControlsViewModels
{
    /// <summary>
    /// View model for a search bar
    /// </summary>
    public class SearchBarViewModel : BaseViewModel
    {
        private string _search;
        /// <summary>
        /// Text of the search bar
        /// </summary>
        public string Search
        {
            get
            {
                return _search;
            }
            set
            {
                _search = value;
                _callback(value);
                OnPropertyChanged(nameof(Search));
            }
        }

        // function to call when the search changes
        private Action<string> _callback;

        public SearchBarViewModel(Action<string> callback)
        {
            _callback = callback;
        }

    }
}
