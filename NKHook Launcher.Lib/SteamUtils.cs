using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NKHook_Launcher.Lib
{
    public class SteamUtils
    {
        public const UInt64 BTD5AppID = 306020;
        public const string BTD5Name = "BloonsTD5";

        public const UInt64 BTDBAppID = 444640;
        public const string BTDBName = "Bloons TD Battles";

        public const UInt64 BMCAppID = 1252780;
        public const string BMCName = "Bloons Monkey City";

        private static Dictionary<UInt64, string> steamGames = new Dictionary<UInt64, string>
        {{BTD5AppID, BTD5Name}, {BTDBAppID, BTDBName}, {BMCAppID, BMCName}};


        private class Utils
        {
            // Takes any quotation marks out of a string.
            public static string StripQuotes(string str)
            {
                return str.Replace("\"", "");
            }

            public static string UnixToWindowsPath(string UnixPath)
            {
                return UnixPath.Replace("/", "\\");
            }
        }

        public static UInt64 GetGameID(string gameName)
        {
            if (gameName == "BTD5")
                return BTD5AppID;
            if (gameName == "BTDB")
                return BTDBAppID;
            if (gameName == "BMC")
                return BMCAppID;
            return 0;
        }

        public static bool IsGameRunning()
        {
            int isGameRunning = (int)Registry.GetValue(Registry.CurrentUser +
                "\\Software\\Valve\\Steam\\Apps\\" + BTD5AppID, "Running", null);
            if (isGameRunning == 1) // Cant type true because its of type System.Bool.
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetGameDir()
        {
            return GetGameDir(BTD5AppID, BTD5Name);
        }

        public static string GetGameDir(UInt64 appid, string gameName)
        {
            string steamDir = (string)Registry.GetValue(Registry.CurrentUser +
                "\\Software\\Valve\\Steam", "SteamPath", null);
            if (steamDir == null)
            {
                Log.Output("Failed to find steam in registry!");
                return null;
            }

            //
            // Check if game is installed first
            //

            int isGameInstalled = (int)Registry.GetValue(Registry.CurrentUser +
                "\\Software\\Valve\\Steam\\Apps\\" + appid, "Installed", null);
            if (isGameInstalled != 1)
            {
                Log.Output(gameName + " is not installed!");
                return null;
            }

            //
            // Get game Directory...
            //

            string configFileDir = steamDir + "\\steamapps\\libraryfolders.vdf";
            List<string> SteamLibDirs = new List<string>();
            SteamLibDirs.Add(Utils.UnixToWindowsPath(steamDir)); // This steam Directory is always here.
            string[] configFile = File.ReadAllLines(configFileDir);
            for (int i = 0; i < configFile.Length; i++)
            {
                // To Example lines are
                // 	"ContentStatsID"		"-4535501642230800231"
                // "1"     "C:\\SteamLibrary"
                // So, we scan for the items in quotes, if the first one is numeric, 
                // then the second one will be a steam library.
                Regex reg = new Regex("\".*?\"");
                MatchCollection matches = reg.Matches(configFile[i]);
                for (int match = 0; match < matches.Count; match++)
                {
                    if (match == 0)
                    {

                        if (int.TryParse(Utils.StripQuotes(matches[match].Value.ToString()), out int n))
                        {
                            // We dont actually need N, we just need to check if the value is an integer.
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (match == 1)
                    {
                        SteamLibDirs.Add(Utils.UnixToWindowsPath(Utils.StripQuotes(matches[match].Value.ToString())));
                    }
                }
            }
            for (int i = 0; i < SteamLibDirs.Count; i++)
            {
                string GameFolder = (SteamLibDirs[i] + "\\steamapps\\common\\" + gameName).Replace("\\\\", "\\");
                if (Directory.Exists(GameFolder))
                {
                    //Log.Output("Found " + gameName + " directory at: " + GameFolder);
                    return GameFolder;
                }
            }

            //Log.Output(gameName + "'s Directory not found!");
            return null;
        }
    }
}
