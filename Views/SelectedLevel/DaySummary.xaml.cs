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
using Whydoisuck.ViewModels.Session;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.Views.SelectedLevel
{
    /// <summary>
    /// Logique d'interaction pour DaySummary.xaml
    /// </summary>
    public partial class DaySummary : UserControl
    {
        public object[] SessionsOfDay
        {//TODO
            get
            {
                return new[] { new {ViewSessionCommand}, new { ViewSessionCommand }, new { ViewSessionCommand }, new { ViewSessionCommand } };
            }
        }

        public NavigatorCommand ViewSessionCommand { get; set; }


        public DaySummary()
        {
            ViewSessionCommand = new NavigatorCommand(MainWindowViewModel.Instance, new SessionViewModel());
            InitializeComponent();
        }
    }
}
