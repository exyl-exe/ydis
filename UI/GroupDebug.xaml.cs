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
using Whydoisuck.DataSaving;

namespace Whydoisuck.UI
{
    /// <summary>
    /// Logique d'interaction pour GroupDebug.xaml
    /// </summary>
    public partial class GroupDebug : UserControl
    {
        public GroupDebug()
        {
            InitializeComponent();
            refreshButton.Click += RefreshList;
        }

        private void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void RefreshList(object sender, EventArgs e)
        {
            GroupList.ItemsSource = GroupLoader.GetAllGroups();
        }
    }
}
