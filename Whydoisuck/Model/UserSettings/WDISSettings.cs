using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydis.Properties;

namespace Ydis.Model.UserSettings
{
    /// <summary>
    /// Contains settings for the application
    /// </summary>
    public static class WDISSettings
    {
        /// <summary>
        /// Scan period when privileging performance
        /// </summary>
        public const int PERFORMANCE_PERIOD = 100;
        /// <summary>
        /// Scan period when privileging accuracy
        /// </summary>
        public const int ACCURACY_PERIOD = 10;
        /// <summary>
        /// Version number for data not matching any version
        /// </summary>
        public const int INVALID_VERSION = -1;
        /// <summary>
        /// Name of the launcher process
        /// </summary>
        public const string LAUNCHER_PROCESS_NAME = "WhydoisuckLauncher";
        /// <summary>
        /// File name for a shortcut to the launcher
        /// </summary>
        public const string LAUNCHER_SHORTCUT_NAME = "WhydoisuckLauncher.url";
        /// <summary>
        /// File name for a shortcut to the launcher
        /// </summary>
        public const string DEFAULT_LAUNCHER_LOCATION = ".\\WhydoisuckLauncher.exe";
        /// <summary>
        /// Returns the path of the launcher exe file
        /// </summary>
        public static string LauncherPath => DEFAULT_LAUNCHER_LOCATION;
        /// <summary>
        /// Current version of the app, for serialized data
        /// </summary>
        public const int SerializationVersion = 3;
        /// <summary>
        /// Default folder for data saving
        /// </summary>
        public static string SavesPath {
            get
            {
                return CurrentSettings.SavesPath;
            }
            set
            {
                CurrentSettings.SavesPath = value;
                Save();
            }
        }
        /// <summary>
        /// How many milliseconds there are between each scan of the game's memory
        /// </summary>
        public static int ScanPeriod
        {
            get
            {
                return CurrentSettings.ScanPeriod;
            }
            set
            {
                CurrentSettings.ScanPeriod = value;
                Save();
            }
        }

        /// <summary>
        /// Name of the file referencing every folder containing data
        /// </summary>
        public static string SaveManagerFileName => "indexedLevels.wdis";
        /// <summary>
        /// Path of the folder to save crash logs in
        /// </summary>
        public static string LogsPath => "./logs/";
        /// <summary>
        /// How many milliseconds there are between each attempt to attach WDIS to the game's process
        /// </summary>
        public static int AttachPeriod { get; private set; } = 1000;
        /// <summary>
        /// How wide is each part in the level by default (in %)
        /// </summary>
        public static float DefaultPartWidth { get; private set; } = 1;
        /// <summary>
        /// Application data directory path
        /// </summary>
        public static string AppData { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Whydoisuck");
        /// <summary>
        /// Directory to save backups
        /// </summary>
        public static string BackupsPath => Path.Combine(AppData, "Backups");
        public static string TempSaveFolder => Path.Combine(Path.GetTempPath(),"WhyDoISuckTempSaves");
        /// <summary>
        /// Path to a file containing the path of the application (used by the launcher to know where the exe is)
        /// </summary>
        public static string ExeLocationPath { get; } = Path.Combine(AppData, "path.dat");

        // Path to the user settings
        private static string SettingsPath { get; } = Path.Combine(AppData, "config.json");
        // User settings object
        private static SerializedSettings _settings;
        private static SerializedSettings CurrentSettings
        {
            get
            {
                if(_settings == null)
                {
                    if (File.Exists(SettingsPath))
                    {
                        try
                        {
                            var data = File.ReadAllText(SettingsPath);
                            _settings = JsonConvert.DeserializeObject<SerializedSettings>(data);
                        } catch
                        {
                            _settings = new SerializedSettings();
                        }                        
                    } else
                    {
                        _settings = new SerializedSettings();
                    }
                }
                return _settings;
            }
        }

        /// <summary>
        /// Saves the settings on the disk
        /// </summary>
        public static void Save()
        {
            if (!Directory.Exists(AppData))
            {
                Directory.CreateDirectory(AppData);
            }
            File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(CurrentSettings));
        }
    }
}
