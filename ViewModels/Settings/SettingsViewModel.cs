using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.Properties;

namespace Whydoisuck.ViewModels.AppSettings
{
    public class SettingsViewModel : BaseViewModel
    {
        public string TitleText => Resources.SettingsTitle;
        public string PerformanceModeText => Resources.SettingsPerformanceMode;
        public string SaveLocation => Path.GetFullPath(SerializationManager.SaveDirectory);
        public string SaveLocationText => Resources.SettingsSaveLocation;
        public string StartUpText => Resources.SettingsOnStart;
    }
}
