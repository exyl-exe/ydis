using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Whydoisuck.DataSaving;

namespace Whydoisuck
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow instance;

        public MainWindow()
        {
            instance = this;
            InitializeComponent();
            mainGrid.MouseDown += MouseUpCb;
            var recorder = new Recorder();
            recorder.StartRecording();
        }

        private void MouseUpCb(object sender, MouseButtonEventArgs e)
        {
            mainTextBlock.Text = TempLogger.Flush();
        }
        
        public static void SetText(string s)
        {
            instance.mainTextBlock.Text = s;
        }

    }
}
