using System;
using System.Windows;
using Whydoisuck.DataSaving;
using Whydoisuck.ViewModels;

namespace Whydoisuck.Views
{
    /// <summary>
    /// Code-behind for main window
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Recorder _recorder;
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
            Closing += ApplicationExit;

            _recorder = new Recorder();
            _recorder.StartRecording();
        }

        private void ApplicationExit(object sender, EventArgs e)
        {
            _recorder?.StopRecording();
        }
    }
}
