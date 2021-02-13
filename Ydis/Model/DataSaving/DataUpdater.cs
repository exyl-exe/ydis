using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Ydis.Model.DataStructures;
using Ydis.Model.UserSettings;
using Ydis.Model.Utilities;

namespace Ydis.Model.DataSaving
{
    /// <summary>
    /// Class to update data in a given directory
    /// </summary>
    public static class DataUpdater
    {
        public static void TryUpdate(string dir, bool canBackup)
        {
            var version = GetDataVersion(dir);
            if (version == WDISSettings.INVALID_VERSION)
            {
                DirectoryUtilities.DeleteDirectoryContent(dir);
                return;
            }
            if(version < WDISSettings.SerializationVersion)
            {
                if (canBackup)
                {
                    BackupManager.Backup(dir);
                }
                var updatedProperly = Update(dir, version);
                if (!updatedProperly)
                {
                    DirectoryUtilities.DeleteDirectoryContent(dir);
                }
            } else if(version > WDISSettings.SerializationVersion) {
                throw new Exception("Incompatible data version");
            };
        }

        //Gets the version of the data in a given directory
        private static int GetDataVersion(string dir)
        {
            var sessionManagerPath = Path.Combine(dir, WDISSettings.SaveManagerFileName);
            if (!File.Exists(sessionManagerPath)) return WDISSettings.INVALID_VERSION;
            var rawData = File.ReadAllText(sessionManagerPath);
            try
            {
                var json = JObject.Parse(rawData);
                return (int)json[WDISSerializable.VersionPropertyName];
            } catch (JsonReaderException)
            {
                return WDISSettings.INVALID_VERSION;
            }
        }

        // Upgrades the data to the latest version
        private static bool Update(string dir, int ver)
        {
            int dataVersion = ver;
            bool correctFormat = true;
            while(dataVersion != WDISSettings.SerializationVersion && correctFormat)
            {
                switch (dataVersion)
                {
                    case 2:
                        correctFormat = Upgrade2to3(dir);
                        if (correctFormat) dataVersion = 3;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return correctFormat;
        }

        //Upgrades the data from serialization version 2 to version 3
        private static bool Upgrade2to3(string dir)
        {
            void changeSerializationVersion(string managerPath)
            {
                // Update session manager
                var rawManagerData = File.ReadAllText(managerPath);
                var json = JObject.Parse(rawManagerData);
                json[WDISSerializable.VersionPropertyName] = "3";
                File.WriteAllText(managerPath, json.ToString());
            }

            bool validateSession(JObject sessionObject)
            {
                try
                {
                    // dsl
                    var sessionFormat = "{'type':'object','default':{},'required':['Version','SessionName','Level','IsCopyRun','StartTime','Duration','StartPercent','Attempts'],'properties':{'Version':{'type':'integer'},'SessionName':{'type':'string',},'Level':{'type':'object','required':['ID','IsOnline','OriginalID','IsOriginal','Name','Revision','PhysicalLength','IsCustomMusic','MusicID','OfficialMusicID','MusicOffset'],'properties':{'ID':{'type':'integer',},'IsOnline':{'type':'boolean',},'OriginalID':{'type':'integer',},'IsOriginal':{'type':'boolean',},'Name':{'type':'string',},'Revision':{'type':'integer',},'PhysicalLength':{'type':'number',},'IsCustomMusic':{'type':'boolean',},'MusicID':{'type':'integer',},'OfficialMusicID':{'type':'integer',},'MusicOffset':{'type':'number',}},'additionalProperties':true},'IsCopyRun':{'type':'boolean',},'StartTime':{'type':'string',},'Duration':{'type':'string',},'StartPercent':{'type':'number',},'Attempts':{'type':'array','additionalItems':true,'items':{'anyOf':[{'type':'object','required':['Number','EndPercent'],'properties':{'Number':{'type':'integer',},'EndPercent':{'type':'number',}},'additionalProperties':true}]}}},'additionalProperties':true}";
                    JSchema schema = JSchema.Parse(sessionFormat);
                    return sessionObject.IsValid(schema);
                }
                catch {
                    return false;
                }
            }

            var sessionManagerPath = Path.Combine(dir, WDISSettings.SaveManagerFileName);
            try
            {
                List<string> folders = Directory.GetDirectories(dir).ToList();
                foreach(var f in folders)
                {
                    var sessionFiles = Directory.GetFiles(f);
                    var newFolderObject = new JObject();
                    var sessions = new JArray();
                    foreach(var s in sessionFiles)
                    {
                        try
                        {
                            var sRawData = File.ReadAllText(s);
                            var sJson = JObject.Parse(sRawData);
                            if (validateSession(sJson))
                            {
                                sessions.Add(sJson);
                            }
                        }
                        catch (Exception) { }
                    }
                    newFolderObject["Sessions"] = sessions;
                    Directory.Delete(f, true);
                    File.WriteAllText(f, newFolderObject.ToString());
                }
                changeSerializationVersion(sessionManagerPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
