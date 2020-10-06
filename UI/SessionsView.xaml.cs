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
    /// Logique d'interaction pour SessionsView.xaml
    /// </summary>
    public partial class SessionsView : UserControl
    {
        public static DependencyProperty GroupProperty = DependencyProperty.Register("SessionViewGroup",
                                       typeof(SessionGroup),
                                       typeof(SessionsView),
                                       new PropertyMetadata(DataChangeCallback));
        
        private static SessionsView Instance { get; set; }

        public SessionGroup SessionViewGroup
        {
            get { return (SessionGroup)GetValue(GroupProperty); }
            set
            {
                SetValue(GroupProperty, value);
            }
        }

        public SessionsView()
        {
            InitializeComponent();
        }

        private static void DataChangeCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var value = (SessionGroup)args.NewValue;
            if (value == null && Instance != null) return;
        }

        private void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
