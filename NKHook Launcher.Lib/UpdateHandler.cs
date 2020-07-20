using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NKHook_Launcher.Lib
{
    public class UpdateHandler
    {
        string gitAPI = "https://api.github.com/repos/TDToolbox/NKHook-Launcher/releases";
        string gitText = "";

        /// <summary>
        /// Main updater method. Handles all update related functions for ease of use.
        /// </summary>
        public static void HandleUpdates()
        {
            DeleteUpdater();    //delete updater if found to keep directory clean and prevent using old updater

            UpdateHandler update = new UpdateHandler();
            if (!update.IsUpdate())
                return;

            Log.Output("An update is available for NKHook Laucher. Downloading latest version...");
            update.DownloadUpdates();
            update.ExtractUpdater();

            Log.Output("Closing NKHook Launcher...");
            update.LaunchUpdater();
        }

        /// <summary>
        /// Reads gitApi text and gets the latest release version
        /// </summary>
        /// <returns>an int of the latest release version, as a whole number without decimals</returns>
        private int GetLatestVersion()
        {
            gitText = WebHandler.ReadText_FromURL(gitAPI);
            if (!Guard.IsStringValid(gitText))
            {
                Log.Output("Failed to read release info for NKHook5 Launcher");
                return 0;
            }

            var gitApi = GitApi.FromJson(gitText);
            string latestRelease = gitApi[0].TagName;
            
            return int.Parse(latestRelease.Replace(".", ""));   
        }

        /// <summary>
        /// Reads gitApi text and gets all of the download urls associated with the latest release
        /// </summary>
        /// <returns>a list of download url strings</returns>
        private List<string> GetDownloadURLs()
        {
            if (!Guard.IsStringValid(gitText))
            {
                gitText = WebHandler.ReadText_FromURL(gitAPI);
                if (!Guard.IsStringValid(gitText))
                {
                    Log.Output("Failed to read release info for NKHook5 Launcher");
                    return null;
                }
            }

            List<string> downloads = new List<string>();
            var gitApi = GitApi.FromJson(gitText);
            foreach (var a in gitApi[0].Assets)
                downloads.Add(a.BrowserDownloadUrl.ToString());

            return downloads;
        }

        /// <summary>
        /// Check if there is an update
        /// </summary>
        /// <returns>true or false, whether or not there is an update</returns>
        public bool IsUpdate()
        {
            int latestVersion = GetLatestVersion();
            int currentVersion = int.Parse(FileVersionInfo.GetVersionInfo(Environment.CurrentDirectory + "\\NKHook Launcher.exe").FileVersion.Replace(".", ""));

            if (currentVersion.ToString().Length > latestVersion.ToString().Length)
            {
                while (currentVersion.ToString().Length > latestVersion.ToString().Length)
                    latestVersion *= 10;
            }

            if (latestVersion > currentVersion)
                return true;
            
            return false;
        }

        /// <summary>
        /// Download all files aquired from the GetDownloadURLs method
        /// </summary>
        public void DownloadUpdates()
        {
            List<string> downloads = GetDownloadURLs();
            foreach (string file in downloads)
            {
                WebHandler.DownloadFile(file, Environment.CurrentDirectory);
            }    
        }

        /// <summary>
        /// Extract the updater from the downloaded zip
        /// </summary>
        public void ExtractUpdater()
        {
            var files = Directory.GetFiles(Environment.CurrentDirectory);
            foreach (var file in files)
            {
                if (!file.EndsWith(".zip") && !file.EndsWith(".rar") && !file.EndsWith(".7z"))
                    continue;

                using (ZipArchive archive = ZipFile.OpenRead(file))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (!entry.FullName.ToLower().Contains("update"))
                            continue;

                        string destinationPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, entry.FullName));
                        if (destinationPath.StartsWith(Environment.CurrentDirectory, StringComparison.Ordinal))
                        {
                            if (File.Exists(destinationPath))
                                File.Delete(destinationPath);

                            entry.ExtractToFile(destinationPath);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Launch the updater exe so the update can continue.
        /// </summary>
        public void LaunchUpdater()
        {
            string updater = Environment.CurrentDirectory + "\\Updater for NKHook Launcher.exe";

            if (!File.Exists(updater))
            {
                Log.Output("ERROR! Unable to find updater. You will need to close the NKHook Launcher " +
                    "and manually extract NKHook Launcher.zip");
                return;
            }

            Process.Start(updater);
        }

        /// <summary>
        /// Delete all files related to updater. Used to keep program directory clean
        /// </summary>
        public static void DeleteUpdater()
        {
            if (File.Exists(Environment.CurrentDirectory + "\\Updater for NKHook Launcher.exe"))
                File.Delete(Environment.CurrentDirectory + "\\Updater for NKHook Launcher.exe");

            var files = Directory.GetFiles(Environment.CurrentDirectory);
            foreach (var file in files)
            {
                FileInfo f = new FileInfo(file);
                if (f.Name.ToLower().Replace(".", "").Replace(" ", "") == "nkhooklauncherzip")
                {
                    File.Delete(file);
                    break;
                }

            }
        }
    }
}
