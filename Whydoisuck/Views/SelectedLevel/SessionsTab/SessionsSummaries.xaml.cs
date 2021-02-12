using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Whydoisuck.ViewModels.SelectedLevel.SessionsTab;

namespace Whydoisuck.Views.SelectedLevel.SessionsTab
{
    /// <summary>
    /// Code-behind for SessionSummaries.xaml
    /// </summary>
    public partial class SessionsSummaries : UserControl
    {
        private bool _loaded = false;

        public SessionsSummaries() {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as SessionsSummariesViewModel;
            SessionsScrollViewer.ScrollToVerticalOffset(vm.ScrollValue);
            _loaded = true;
        }

        private void SessionsScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_loaded)
            {
                var vm = DataContext as SessionsSummariesViewModel;
                vm.ScrollValue = e.VerticalOffset;
            }            
        }
    }
}
