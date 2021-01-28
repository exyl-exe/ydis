using System;
using System.Collections.Generic;
using System.IO;

namespace Whydoisuck.Model.Utilities
{
    /// <summary>
    /// Custom directory operations
    /// </summary>
    public static class DirectoryUtilities
    {
        public static void Copy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            copyFiles(sourceDirName, destDirName);

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    Copy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        public static void MoveDirectoryContent(string sourceDirectory, string destDirectory)
        {
            Directory.CreateDirectory(destDirectory);
            moveFiles(sourceDirectory, destDirectory);
            Directory.Delete(sourceDirectory, true);
        }

        private static void copyFiles(string sourceDirName, string destDirName)
        {
            if (!Directory.Exists(sourceDirName)) return;
            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                if (!File.Exists(tempPath))
                {
                    file.CopyTo(tempPath, false);
                }
                else
                {
                    var i = 1;
                    string renamedFilePath;
                    do
                    {
                        renamedFilePath = string.Format("{0}{1}", tempPath, i);
                        i++;
                    } while (File.Exists(renamedFilePath));
                    file.CopyTo(renamedFilePath, false);
                }
            }
        }

        // Get the files in the directory and copy them to the new location.
        private static void moveFiles(string sourceDirName, string destDirName)
        {
            if (!Directory.Exists(sourceDirName) || !Directory.Exists(destDirName)) return;
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                if (!File.Exists(tempPath))
                {
                    file.MoveTo(tempPath);
                }
                else
                {
                    var i = 1;
                    string renamedFilePath;
                    do
                    {
                        renamedFilePath = string.Format("{0}{1}", tempPath, i);
                        i++;
                    } while (File.Exists(renamedFilePath));
                    file.CopyTo(renamedFilePath, false);
                }
            }
        }
    }
}
