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
    public class MainWindowViewModel : BaseViewModel, ReplaceableViewViewModel
    {
		public NavigationPanelViewModel NavigationPanel { get; set; }
		public BaseViewModel CurrentView { get; set; }
		public Recorder Recorder { get; set; }

		public MainWindowViewModel(Recorder recorder)
		{
			Recorder = recorder;
			NavigationPanel = new NavigationPanelViewModel(this);
			CurrentView = new CurrentLevelViewModel(recorder);
		}

		public void ReplaceView(BaseViewModel m)
		{
			CurrentView = m;
			OnPropertyChanged(nameof(CurrentView));
		}
	}
}
