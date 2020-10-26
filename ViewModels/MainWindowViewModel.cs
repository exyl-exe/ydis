using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.ViewModels.CurrentLevel;
using Whydoisuck.ViewModels.Navigation;
using Whydoisuck.ViewModels.SelectedLevel;
using Whydoisuck.Views.Currentlevel;

namespace Whydoisuck.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
		public NavigationPanelViewModel NavigationPanel { get; set; }
		public BaseViewModel CurrentMainView { get; set; }
		public SessionManager Manager { get; set; }

		public MainWindowViewModel(SessionManager manager)
		{
			Manager = manager;
			NavigationPanel = new NavigationPanelViewModel(this);
			CurrentMainView = new CurrentLevelViewModel();
		}

		public void ReplaceMainView(BaseViewModel m)
		{
			CurrentMainView = m;
			OnPropertyChanged(nameof(CurrentMainView));
		}
	}
}
