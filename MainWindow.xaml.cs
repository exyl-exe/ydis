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
using Whydoisuck.MemoryReading;

namespace Whydoisuck
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GDMemoryReader reader;

        public MainWindow()
        {
            reader = new GDMemoryReader();
            InitializeComponent();
            mainGrid.MouseDown += MouseUpCb;
        }

        private void MouseUpCb(object sender, MouseButtonEventArgs e)
        {
            if (!reader.IsInitialized)
            {
                var success = reader.Initialize();
                if (!success)
                {
                    mainTextBlock.Text = "Not opened";
                    return;
                }
            }
            reader.Update();
            mainTextBlock.Text = reader.GetState();
        }
    }
}
