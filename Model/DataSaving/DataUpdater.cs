using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Model.UserSettings;

namespace Whydoisuck.Model.DataSaving
{
    /// <summary>
    /// Class to update data in a given directory
    /// </summary>
    public static class DataUpdater
    {
        public static void Update(string dir)
        {
            var version = GetDataVersion(dir);
        }

        //Gets the version of the data in a given directory
        private static int GetDataVersion(string dir)
        {
            var sessionManagerPath = Path.Combine(dir, WDISSettings.SaveManagerFileName);
            var rawData = File.ReadAllText(sessionManagerPath);
            var json = JObject.Parse(rawData);
            return json[WDISSerializable.VersionPropertyName].Value<int>();
        }
    }
}
