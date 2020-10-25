using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Whydoisuck.DataModel;

namespace Whydoisuck.ViewModels.SelectedLevel
{
    public class SessionsTabViewModel : BaseViewModel
    {
        public BaseViewModel CurrentView => Views.Peek();
        private Stack<BaseViewModel> Views { get; set; }

        public SessionsTabViewModel(SessionGroup g)
        {
            Views = new Stack<BaseViewModel>();
            Views.Push(new SessionsSummariesViewModel(this, g));
        }

        public void PushView(BaseViewModel m)
        {
            Views.Push(m);
            OnPropertyChanged(nameof(CurrentView));
        }

        public void PopView()
        {
            Views.Pop();
            OnPropertyChanged(nameof(CurrentView));
        }
    }
}
