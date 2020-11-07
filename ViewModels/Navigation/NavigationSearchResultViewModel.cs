using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.SelectedLevel;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.ViewModels.Navigation
{
    public class NavigationSearchResultViewModel
    {
        public NavigatorCommand UpdateCommand { get; set; }
        public SessionGroup Group { get; set; }
        private SelectedLevelViewModel SelectedView { get; set; }

        public NavigationSearchResultViewModel(SessionGroup group, MainWindowViewModel mainView, SelectedLevelViewModel selectedView)
        {
            Group = group;
            SelectedView = selectedView;
            UpdateCommand = new NavigatorCommand(mainView, selectedView);
        }

        public void Update()
        {
            SelectedView.Update();
        }
    }
}
