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
using Whydoisuck.DataModel;

namespace Whydoisuck.UI
{
    /// <summary>
    /// Logique d'interaction pour GroupView.xaml
    /// </summary>
    public partial class GroupView : UserControl
    {
        public static DependencyProperty GroupProperty =
        DependencyProperty.Register("GroupViewSessionGroup", typeof(SessionGroup), typeof(GroupView));
        public SessionGroup GroupViewSessionGroup {
            get { return (SessionGroup)GetValue(GroupProperty); }
            set { SetValue(GroupProperty, value);}
        }

        public GroupView()
        {
            InitializeComponent();
        }
    }
}
