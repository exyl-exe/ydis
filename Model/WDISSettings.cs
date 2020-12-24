using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Model
{
    /// <summary>
    /// Contains settings for the application
    /// </summary>
    public static class WDISSettings
    {
        /// <summary>
        /// Default folder for data saving
        /// </summary>
        public static string DefaultSavePath { get; private set; } = "./records/";//TODO appdata ?
        /// <summary>
        /// Name of the file referencing every folder containing data
        /// </summary>
        public static string SaveManagerFileName => "indexedLevels.wdis";
        /// <summary>
        /// Path of the folder to save crash logs in
        /// </summary>
        public static string LogsPath => "./logs/";
        /// <summary>
        /// How many milliseconds there are between each scan of the game's memory
        /// </summary>
        public static int ScanPeriod { get; private set; } = 10;
        /// <summary>
        /// How many milliseconds there are between each attempt to attach WDIS to the game's process
        /// </summary>
        public static int AttachPeriod { get; private set; } = 1000;
        /// <summary>
        /// How wide is each part in the level by default (in %)
        /// </summary>
        public static float DefaultPartWidth { get; private set; } = 1;
    }
}
