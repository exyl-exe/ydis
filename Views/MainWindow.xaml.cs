using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Whydoisuck.DataSaving;
using Whydoisuck.Model.Utilities;
using Whydoisuck.Properties;
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
            Application.Current.DispatcherUnhandledException += ExceptionExit;
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

        private void ExceptionExit(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                ExceptionLogger.Log(sender, e);
                Closing -= ApplicationExit;
                _recorder?.CrashRecorder();
                string msg = string.Format("{0}\n{1}", Properties.Resources.ErrorMessage,
                                                       string.Format(Properties.Resources.ErrorMessageLogLocationFormat, Settings.Default.LogsPath));
                MessageBox.Show(msg, Properties.Resources.ErrorMessageTitle);
                e.Handled = true;
            } catch { } // Probably prevents looping if an error occurs while logging the error
            Application.Current.Shutdown();
        }
    }
}
