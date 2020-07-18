using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NKHook_Launcher.Lib
{
    class WebHandler
    {
        WebHandler reader;

        bool exitLoop = false;
        public string startURL { get; set; }
        public string readURL { get; set; }
        public string LatestVersionNumber { get; set; }
        public bool urlAquired { get; set; }

        public void DownloadFile(string filename, string url, string savePath, string replaceText, int linenumber)
        {
            Log.Output("Downloading latest " + filename + "...");
            
            WebClient client = new WebClient();
            string git_Text = ReadText_FromURL(url);
            var gitText_Split = git_Text.Split(new[] { '\r', '\n' });

           
           //string updaterURL = processGit_Text(git_Text, replaceText, linenumber);

            /*if (File.Exists(Environment.CurrentDirectory + "\\" + filename))
                File.Delete(Environment.CurrentDirectory + "\\" + filename);*/


            foreach (var link in gitText_Split)
            {
                if (!Guard.IsStringValid(link))
                    continue;

                string[] split = link.Split('\\');
                string file = split[split.Length - 1];
                string downloadPath = Environment.CurrentDirectory + "\\avc";// + "\\" + file;

                string testURL = "https://github.com/DisabledMallis/NKHook5/releases/download/pre-1/NKHook5-Injector.exe";
                for (int i = 0; i < 150; i++)
                {
                    try 
                    {
                        MessageBox.Show(link);
                        client.DownloadFile(testURL, downloadPath);
                        File.Move(downloadPath, savePath + "\\" + file);
                        //File.Delete(downloadPath);
                        break; 
                    }
                    catch { throw; }
                    Thread.Sleep(50);
                }
            }
            
            Log.Output(filename + " successfully downloaded!");
        }


        private string ReadText_FromURL(string url)
        {
            string result = "";
            WebClient client = new WebClient();

            for (int i = 0; i <= 250; i++)
            {
                Thread.Sleep(100);
                try
                {
                    if (exitLoop)
                        break;

                    readURL = client.DownloadString(url);
                    if (!Guard.IsStringValid(readURL))
                        continue;

                    urlAquired = true;
                    result = readURL;
                    break;
                }
                catch { }
            }
            return result;
        }


       /* /// <summary>
        /// Complete method. Waits on URL then reads and returns text from it
        /// </summary>
        /// <param name="url">url to read text from</param>
        /// <returns>text read off of website</returns>
        public string WaitOn_URL(string url)   //call this one to read the text from the git url
        {
            WebHandler get = new WebHandler();
            get.startURL = url;
            get.ReadText_FromURL(startURL);

            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(100);
                if (get.urlAquired)
                    break;
            }
            return get.readURL;
        }*/


        public string processGit_Text(string url, string deleteText, int lineNumber)    //call this one read git text and return the url we want. Delete text is the starting word, for example "toolbox2019: "
        {
            if (!Guard.IsStringValid(url))
                return null;

            string[] split = url.Split('\n');
            return split[lineNumber].Replace(deleteText, "");
        }
    }
}
