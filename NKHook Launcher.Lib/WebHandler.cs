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
        private static WebClient client;

        /// <summary>
        /// Download file from URL
        /// </summary>
        /// <param name="url">download url</param>
        /// <param name="dest">file destination. Just needs directory, doesn't need file name</param>
        public static void DownloadFile(string url, string dest)
        {
            if (client == null)
                client = new WebClient();

            string tempDir = Environment.CurrentDirectory + "\\temp";
            string tempDest = tempDir + "\\file";

            string[] split = url.Split('/');
            string file = split[split.Length-1];
            dest = dest + "\\" + file;

            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            for (int i = 0; i < 150; i++)
            {
                try{ client.DownloadFile(url, tempDest); break; }
                catch { }
                Thread.Sleep(50);
            }

            if (File.Exists(dest))
                File.Delete(dest);

            File.Move(tempDest, dest);
            Directory.Delete(tempDir, true);
        }


        /// <summary>
        /// Downloads string of text from the URL
        /// </summary>
        /// <param name="url">URL to get string of text from</param>
        /// <returns>String of text or nothing</returns>
        public static string ReadText_FromURL(string url)
        {
            string result = "";
            WebClient client = new WebClient();
            client.Headers.Add("user-agent", " Mozilla/5.0 (Windows NT 6.1; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0");

            for (int i = 0; i <= 250; i++)
            {
                Thread.Sleep(100);
                try
                {
                    result = client.DownloadString(url);
                    
                    if (!Guard.IsStringValid(result))
                        continue;

                    break;
                }
                catch { throw; }
            }
            return result;
        }
    }
}
