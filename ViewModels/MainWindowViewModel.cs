using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.ViewModels.Current;
using Whydoisuck.ViewModels.Navigation;
using Whydoisuck.ViewModels.SelectedLevel;
using Whydoisuck.Views.Currentlevel;

namespace Whydoisuck.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
		public NavigationPanelViewModel NavigationPanel { get; set; }


		private readonly Stack<BaseViewModel> _mainView;
		public BaseViewModel CurrentMainView
		{
			get { return _mainView.Peek(); }
		}

		public MainWindowViewModel()
		{
			NavigationPanel = new NavigationPanelViewModel(this);
			_mainView = new Stack<BaseViewModel>();
			_mainView.Push(new CurrentLevelViewModel());
		}

		public void ReplaceMainView(BaseViewModel m)
		{
			_mainView.Pop();
			_mainView.Push(m);
			OnPropertyChanged(nameof(CurrentMainView));
		}
	}
}
