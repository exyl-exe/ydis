using System;
using System.Windows;
using System.Windows.Input;
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
            _recorder = new Recorder();
            _recorder.StartRecording();
            DataContext = new MainWindowViewModel(_recorder);
            InitializeComponent();
            Closing += ApplicationExit;

        }

        private void ApplicationExit(object sender, EventArgs e)
        {
            _recorder?.StopRecording();
        }
    }
}
