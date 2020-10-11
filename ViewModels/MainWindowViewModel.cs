using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.ViewModels.Current;
using Whydoisuck.ViewModels.SelectedLevel;
using Whydoisuck.Views.Currentlevel;

namespace Whydoisuck.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
		private readonly Stack<BaseViewModel> _mainView;
		public BaseViewModel CurrentMainView
		{
			get { return _mainView.Peek(); }
		}

		public MainWindowViewModel()
		{
			Instance = this;
			_mainView = new Stack<BaseViewModel>();
			_mainView.Push(new SelectedLevelViewModel());
		}

		public void ReplaceMainView(BaseViewModel m)
		{
			_mainView.Pop();
			_mainView.Push(m);
			OnPropertyChanged(nameof(CurrentMainView));
		}


		//TODO remove below
		public static MainWindowViewModel Instance { get; set; }
	}
}
