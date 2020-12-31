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
            if(version < WDISSettings.SerializationVersion)
            {
                Upgrade(dir, version);
            } else if(version > WDISSettings.SerializationVersion) {
                throw new Exception("Incompatible data version");
            }
        }

        //Gets the version of the data in a given directory
        private static int GetDataVersion(string dir)
        {
            var sessionManagerPath = Path.Combine(dir, WDISSettings.SaveManagerFileName);
            var rawData = File.ReadAllText(sessionManagerPath);
            var json = JObject.Parse(rawData);
            return (int)json[WDISSerializable.VersionPropertyName];
        }

        // Upgrades the data to the latest version
        private static void Upgrade(string dir, int ver)
        {
            int currentVersion = ver;
            while(currentVersion != WDISSettings.SerializationVersion)
            {
                switch (currentVersion)
                {
                    case 2:
                        Upgrade2to3(dir);
                        currentVersion = 3;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        //Upgrades the data from serialization version 2 to version 3
        private static void Upgrade2to3(string dir)
        {
            throw new NotImplementedException();
        }
    }
}
