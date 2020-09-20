﻿using System;
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
        private Recorder recorder;
        public static RoutedCommand LogCommand = new RoutedCommand();
        public MainWindow()
        {
            InitializeComponent();
            Closing += ApplicationExit;

            LogCommand.InputGestures.Add(new KeyGesture(Key.L, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(LogCommand, LogsVisibility));

            LogsPanel.MouseDown += RefreshLogs;
            AllGroups.GroupList.ItemsSource = GroupLoader.GetAllGroups();
            
            recorder = new Recorder();
            recorder.StartRecording();
        }

        private void ApplicationExit(object sender, EventArgs e)
        {
            recorder.StopRecording();
        }



        private void RefreshLogs(object sender, MouseButtonEventArgs e)
        {
            LogsTextBlock.Text = TempLogger.Flush();
        }

        private void LogsVisibility(object sender, ExecutedRoutedEventArgs e)
        {
            if(LogsView.Visibility == Visibility.Collapsed)
            {
                LogsView.Visibility = Visibility.Visible;
            } else
            {
                LogsView.Visibility = Visibility.Collapsed;
            }
        }
    }
}