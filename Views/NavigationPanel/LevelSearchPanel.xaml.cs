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
using Whydoisuck.ViewModels;
using Whydoisuck.ViewModels.Navigation;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.Views.NavigationPanel
{
    /// <summary>
    /// Logique d'interaction pour LevelSearchPanel.xaml
    /// </summary>
    public partial class LevelSearchPanel : UserControl
    {
        public ICommand UpdateNavigatorView { get; set; }

        public LevelSearchPanel()
        {
            InitializeComponent();
            DataContext = new NavigationSearchViewModel();

            UpdateNavigatorView = new NavigatorCommand(MainWindowViewModel.Instance);

            var searchRes = new List<object>();
            foreach (var l in "abcdefghijklmnopqrstuvwxyzhecjhjhdsb")
            {
                searchRes.Add(new {text = l.ToString(), UpdateNavigatorView });
            }
            DataContext = new { SearchResults = searchRes };
        }
    }
}
