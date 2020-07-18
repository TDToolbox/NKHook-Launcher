using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NKHook_Launcher.Lib
{
    public class NKHook
    {
        public static string nkhDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NKHook5";
        public static string nkhEXE = nkhDir + "\\NKHook5-Injector.exe";

        public static void OpenNkhDir()
        {
            if(!DoesNkhExist())
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
    }    
}
