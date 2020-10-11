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

namespace Whydoisuck.Views.SelectedLevel
{
    /// <summary>
    /// Logique d'interaction pour SessionsTab.xaml
    /// </summary>
    public partial class SessionsTab : UserControl
    {

        public int[] Days { get; set; } = new[] { 24, 25, 26, 27 };

        public SessionsTab()
        {
            InitializeComponent();
        }
    }
}
