using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Whydoisuck.Model.DataSaving;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Model.UserSettings;
using Whydoisuck.Model.Utilities;
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
        /// Save files location option name
        /// </summary>
        public string ImportDataText => Resources.SettingsImportData;
        /// <summary>
        /// Save files location option description
        /// </summary>
        public string ImportDataDesc => Resources.SettingsImportDataDesc;
        /// <summary>
        /// Save files location option name
        /// </summary>
        public string SaveLocationText => Resources.SettingsSaveLocation;
        /// <summary>
        /// Save files path
        /// </summary>
        public string SaveLocation => Path.GetFullPath(SessionManager.Instance.SavesDirectory);
        /// <summary>
        /// Change location description
        /// </summary>
        public string SaveLocationDescription => Resources.SettingsSaveLocationDescription;
        /// <summary>
        /// Start up option name
        /// </summary>
        public string StartUpText => Resources.SettingsOnStart;
        /// <summary>
        /// Start up option description
        /// </summary>
        public string StartUpDesc => Resources.SettingsOnStartDesc;
        /// <summary>
        /// Performance mode option name
        /// </summary>
        public string PerformanceModeText => Resources.SettingsPerformanceMode;
        /// <summary>
        /// Performance mode option description
        /// </summary>
        public string PerformanceModeDesc => Resources.SettingsPerformanceModeDesc;       
        /// <summary>
        /// Save files location selector command
        /// </summary>
        public ChangeSavesLocationCommand BrowseCommand { get; private set; }
        /// <summary>
        /// Import files command
        /// </summary>
        public FolderBrowserCommand ImportCommand { get; private set; }
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

        /// <summary>
        /// Controls wether whydoisuck is launched automatically when GD is opened
        /// </summary>
        public bool AutoStartup
        {
            get
            {
                return AutoLaunchUtilities.DoesShortcutExist(WDISSettings.LAUNCHER_SHORTCUT_NAME, WDISSettings.LauncherPath);
            }
            set
            {
                if (value)
                {
                    var path = WDISSettings.LauncherPath;
                    if (File.Exists(path))
                    {
                        Process.Start(path);
                        AutoLaunchUtilities.CreateStartUpShortcut(WDISSettings.LAUNCHER_SHORTCUT_NAME, path);
                    }
                } else
                {                    
                    var processes = Process.GetProcessesByName(WDISSettings.LAUNCHER_PROCESS_NAME);
                    foreach(var p in processes)
                    {
                        p.Kill();
                    }
                    AutoLaunchUtilities.RemoveStartUpShortcut(WDISSettings.LAUNCHER_SHORTCUT_NAME);
                }
            }
        }

        public SettingsViewModel()
        {
            BrowseCommand = new ChangeSavesLocationCommand(UpdatePath);
            ImportCommand = new FolderBrowserCommand(ImportData);
        }

        /// <summary>
        /// Called to import data
        /// </summary>
        private void ImportData(string path)
        {
            if (path == null || path == SessionManager.Instance.SavesDirectory) return;
            SessionManager.Instance.Import(path);
        }

        private void UpdatePath(string path)
        {
            OnPropertyChanged(nameof(SaveLocation));
        }
    }
}
