using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.ViewModels.SelectedLevel.SessionsTab
{
    /// <summary>
    /// View model for the sessions tab
    /// </summary>
    public class SessionsTabMainViewModel : BaseViewModel
    {
        /// <summary>
        /// Currently displayed view in the sessions tab
        /// </summary>
        public BaseViewModel CurrentView => Views.Peek();
        // Stack of which views were opened, to be able to navigate.
        private Stack<BaseViewModel> Views { get; set; }

        public SessionsTabMainViewModel(SessionGroup g)
        {
            Views = new Stack<BaseViewModel>();
            Views.Push(new SessionsSummariesViewModel(this, g));
        }

        /// <summary>
        /// Adds a view to the stack of opened views. Will be displayed above current views.
        /// </summary>
        /// <param name="m">View to add to the stack.</param>
        public void PushView(BaseViewModel m)
        {
            Views.Push(m);
            OnPropertyChanged(nameof(CurrentView));
        }

        /// <summary>
        /// Removes top view from the stack. The underneath view will be displayed instead.
        /// </summary>
        public void PopView()
        {
            Views.Pop();
            OnPropertyChanged(nameof(CurrentView));
        }
    }
}
