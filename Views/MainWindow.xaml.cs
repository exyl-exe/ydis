using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Whydoisuck.DataSaving;
using Whydoisuck.Model;
using Whydoisuck.Model.UserSettings;
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
        private Recorder _recorder;
        private static Mutex _mutex;
        public MainWindow()
        {
            Application.Current.DispatcherUnhandledException += ExceptionApp;
            AppDomain.CurrentDomain.UnhandledException += ExceptionDomain;
            _mutex = new Mutex(true, "GDWhyDoISuckMutex", out var createdNew);
            if (createdNew)
            {
                Closing += FreeMutex;
                Init();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void FreeMutex(object sender, CancelEventArgs e)
        {
            _mutex?.Close();
        }

        private void Init()
        {
            _recorder = new Recorder();
            _recorder.StartRecording();
            DataContext = new MainWindowViewModel(_recorder);
            InitializeComponent();
            Closing += StopWDIS;
        }

        private void StopWDIS(object sender, EventArgs e)
        {
            _recorder?.StopRecording();
        }

        private void ExceptionApp(object sender, DispatcherUnhandledExceptionEventArgs e)
        { 
            e.Handled = true;
            CrashAndLog(e.Exception);
        }

        private void ExceptionDomain(object sender, UnhandledExceptionEventArgs e)
        {
            CrashAndLog((Exception)e.ExceptionObject);
        }

        private void CrashAndLog(Exception e)
        {
            try
            {
                ExceptionLogger.Log(e);
                Closing -= StopWDIS;
                _recorder?.CrashRecorder();
                string msg = string.Format("{0}\n{1}", Properties.Resources.ErrorMessage,
                                                       string.Format(Properties.Resources.ErrorMessageLogLocationFormat, WDISSettings.LogsPath));
                //Thread needed because sometimes the message box is not displayed                
                MessageBox.Show(Application.Current.MainWindow, msg, Properties.Resources.ErrorMessageTitle);
            }
            catch { } // Probably prevents looping if an error occurs while logging the error
            Application.Current.Shutdown();
        }
    }
}
