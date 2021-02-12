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
    /// Code-behind for SelectedLevelView.xaml
    /// </summary>
    public partial class SelectedLevelView : UserControl
    {
        public SelectedLevelView()
        {
            InitializeComponent();
        }

        // To lose focus on enter
        private void GroupName_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (e.Key == Key.Return)
                {
                    if (e.Key == Key.Enter)
                    {
                        textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GroupName.Focus();
            if (e.GetPosition(GroupName).X > 0){
                GroupName.CaretIndex = GroupName.Text.Length;
            } else
            {
                GroupName.CaretIndex = 0;
            }
        }
    }
}
