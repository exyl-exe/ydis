using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydis.Model.UserSettings;
using Ydis.Model.Utilities;

namespace Ydis.Model.DataSaving
{
    /// <summary>
    /// utility class used to backup data
    /// </summary>
    public static class BackupManager
    {
        /// <summary>
        /// Backs up the data in a given directory
        /// </summary>
        public static void Backup(string dir)
        {
            var backupName = string.Format("{0:yyyyMMddHHmmssfffffff}", DateTime.Now);
            var backupPath = Path.Combine(YDISSettings.BackupsPath, backupName);
            DirectoryUtilities.Copy(dir, backupPath, true);
        }
    }
}
