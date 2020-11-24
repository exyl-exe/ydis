using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.ViewModels.CurrentLevel;
using Whydoisuck.ViewModels.Navigation;
using Whydoisuck.ViewModels.SelectedLevel;
using Whydoisuck.Views.CurrentLevel;

namespace Whydoisuck.ViewModels
{
	/// <summary>
	/// ViewModel for the main window. The largest part of the view can be either
	/// the currently played level or a selected level.
	/// </summary>
    public class MainWindowViewModel : BaseViewModel, IReplaceableViewViewModel
    {
		public NavigationPanelViewModel NavigationPanel { get; set; }
		public BaseViewModel CurrentView { get; set; }
		public Recorder Recorder { get; set; } // TODO remove if the recorder becomes a singleton

		public MainWindowViewModel(Recorder recorder)
		{
			Recorder = recorder;
			NavigationPanel = new NavigationPanelViewModel(this);
			CurrentView = new CurrentLevelViewModel(recorder);
		}

		/// <summary>
		/// Method to change the central part of the main window.
		/// </summary>
		/// <param name="m"></param>
		public void ReplaceView(BaseViewModel m)
		{
			CurrentView = m;
			OnPropertyChanged(nameof(CurrentView));
		}
	}
}
