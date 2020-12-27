﻿using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Whydoisuck.DataSaving;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Model.UserSettings;
using Whydoisuck.Properties;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.ViewModels.AppSettings
{
    /// <summary>
    /// View model for the settings view
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        /// <summary>
        /// Title of the view
        /// </summary>
        public string TitleText => Resources.SettingsTitle;
        /// <summary>
        /// Performance mode option name
        /// </summary>
        public string PerformanceModeText => Resources.SettingsPerformanceMode;
        /// <summary>
        /// Performance mode option description
        /// </summary>
        public string PerformanceModeDesc => Resources.SettingsPerformanceModeDesc;
        /// <summary>
        /// Save files location option name
        /// </summary>
        public string SaveLocationText => Resources.SettingsSaveLocation;
        /// <summary>
        /// Save files location option name
        /// </summary>
        public string ImportDataText => Resources.SettingsImportData;
        /// <summary>
        /// Save files location option description
        /// </summary>
        public string ImportDataDesc => Resources.SettingsImportDataDesc;

        /// <summary>
        /// Save files path
        /// </summary>
        public string SaveLocation => Path.GetFullPath(SessionManager.Instance.SavesDirectory);
        /// <summary>
        /// Save files location selector command
        /// </summary>
        public FolderBrowserCommand BrowseCommand { get; private set; }
        /// <summary>
        /// Import files command
        /// </summary>
        public FolderBrowserCommand ImportCommand { get; private set; }
        /// <summary>
        /// Start up option name
        /// </summary>
        public string StartUpText => Resources.SettingsOnStart;
        /// <summary>
        /// Start up option description
        /// </summary>
        public string StartUpDesc => Resources.SettingsOnStartDesc;

        /// <summary>
        /// Controls how many times per seconds the game is scanned
        /// </summary>
        public bool EnhanceAccuracy
        {
            get
            {
                return WDISSettings.ScanPeriod == WDISSettings.ACCURACY_PERIOD;
            }
            set
            {
                WDISSettings.ScanPeriod = value ? WDISSettings.ACCURACY_PERIOD : WDISSettings.PERFORMANCE_PERIOD;
            }
        }

        public SettingsViewModel()
        {
            BrowseCommand = new FolderBrowserCommand(OnSaveFileLocationChanges);
            ImportCommand = new FolderBrowserCommand((path) => Console.WriteLine(path));
        }

        /// <summary>
        /// Called to change the location of save files
        /// </summary>
        public void OnSaveFileLocationChanges(string path)
        {
            if (path == null || path == SessionManager.Instance.SavesDirectory) return;
            WDISSettings.SavesPath = path;
            var migrateData = ShowMigrateDialog(path);
            if (migrateData)
            {
                SessionManager.Instance.SetRootAndMerge(path);
            }
            else
            {
                SessionManager.Instance.SetRoot(path);
            }
        }

        public bool ShowMigrateDialog(string newPath)
        {
            var caption = Resources.MigrateDataCaption;
            var content = string.Format(Resources.MigrateDataContentFormat, newPath);
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            return result == MessageBoxResult.Yes;
        }

    }
}
