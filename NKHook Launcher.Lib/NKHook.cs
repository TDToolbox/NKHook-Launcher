using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using NKHook_Launcher;

namespace NKHook_Launcher.Lib
{
    public class NKHook
    {
        public static string nkhDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NKHook5";
        public static string nkhEXE = nkhDir + "\\NKHook5-Injector.exe";
        public static string gitReleaseInfo = "https://api.github.com/repos/DisabledMallis/NKHook5/releases";

        public static void OpenNkhDir()
        {
            if (!Directory.Exists(nkhDir))
            {
                Log.Output("Error! NKHook is not installed");
                return;
            }

            Process.Start(nkhDir);
        }

        public static bool DoesNkhExist()
        {
            if (File.Exists(nkhEXE))
                return true;

            return false;
        }

        public static void LaunchNKH()
        {
            if (!DoesNkhExist())
            {
                Log.Output("Unable to find NKHook5-Injector.exe. Failed to launch...");
                return;
            }
            Process.Start(nkhEXE);
        }

        public static void DownloadNKH(string filename, string url, string savePath)
        {
            Log.Output("Downloading latest " + filename + "...");

            string git_Text = WebHandler.ReadText_FromURL(url);
            if (!Guard.IsStringValid(git_Text))
            {
                MessageBox.Show("Failed to read release info for " + filename);
                return;
            }

            var gitApi = GitApi.FromJson(git_Text);
            foreach (var a in gitApi)
            {
                foreach (var b in a.Assets)
                {
                    string link = b.BrowserDownloadUrl.ToString();
                    if (!Guard.IsStringValid(link))
                        continue;

                    WebHandler.DownloadFile(link, savePath);
                }
            }
            Log.Output(filename + " successfully downloaded!");
        }
    }    
}
