using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public static void HandleUpdates()
        {
            UpdateHandler update = new UpdateHandler();
            if (!update.IsUpdate())
            {
                return;
            }

            Log.Output("An update is available for NKHook Laucher. Downloading latest version...");
            update.DownloadUpdates();
            update.LaunchUpdater();
        }

        private int GetLatestVersion()
        {
            gitText = WebHandler.ReadText_FromURL(gitAPI);
            if (!Guard.IsStringValid(gitText))
            {
                MessageBox.Show("Failed to read release info for NKHook5 Launcher");
                return 0;
            }

            var gitApi = GitApi.FromJson(gitText);
            string latestRelease = gitApi[0].TagName;
            
            return int.Parse(latestRelease.Replace(".", ""));   
        }

        private List<string> GetDownloadURLs()
        {
            if (!Guard.IsStringValid(gitText))
            {
                gitText = WebHandler.ReadText_FromURL(gitAPI);
                if (!Guard.IsStringValid(gitText))
                {
                    MessageBox.Show("Failed to read release info for NKHook5 Launcher");
                    return null;
                }
            }

            List<string> downloads = new List<string>();
            var gitApi = GitApi.FromJson(gitText);
            foreach (var a in gitApi[0].Assets)
                downloads.Add(a.BrowserDownloadUrl.ToString());

            return downloads;
        }

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

        public void DownloadUpdates()
        {
            List<string> downloads = GetDownloadURLs();
            foreach (string file in downloads)
            {
                WebHandler.DownloadFile(file, Environment.CurrentDirectory);
            }    
        }

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
    }
}
