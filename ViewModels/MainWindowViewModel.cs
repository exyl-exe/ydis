using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.Model.DataSaving;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Model.UserSettings;
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
		/// <summary>
		/// View model for the navigation panel
		/// </summary>
		public NavigationPanelViewModel NavigationPanel { get; set; }
		/// <summary>
		/// View model for the currently displayed view
		/// </summary>
		public BaseViewModel CurrentView { get; set; }
		/// <summary>
		/// Recorder saving attempts
		/// </summary>
		public Recorder Recorder { get; set; }

		private CurrentLevelViewModel CurrentSession { get; set; }

		public MainWindowViewModel(Recorder recorder)
		{
			Recorder = recorder;
			CurrentSession = new CurrentLevelViewModel(recorder);
			NavigationPanel = new NavigationPanelViewModel(this, CurrentSession);
			CurrentView = CurrentSession;
			SessionManager.Instance.OnGroupDeleted += OnGroupDeleted;
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

		/// <summary>
		/// Method to replace the currently displayed view if the currently displayed folder is deleted
		/// </summary>
		public void OnGroupDeleted(SessionGroup g)
		{
			if (CurrentView is DelayedViewModel vm
				&& vm.ViewModel is SelectedLevelViewModel slvm
				&& slvm.ContainsGroup(g))
			{
				ReplaceView(CurrentSession);
			}
		}
	}
}
