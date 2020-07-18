using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Updater_for_NKHook_Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Output("Updater for NKHook5 Launcher has started!");
            CloseWindow("NKHook Launcher");
            ExtractFiles();
            Output("Restarting NKHook5 Launcher...");
            Process.Start(Environment.CurrentDirectory + "\\NKHook Launcher.exe");
        }

        public static void ExtractFiles()
        {
            Output("Extracting files from Update");
            var files = Directory.GetFiles(Environment.CurrentDirectory);
            foreach (var file in files)
            {
                if (!file.EndsWith(".zip") && !file.EndsWith(".rar") && !file.EndsWith(".7z"))
                    continue;

                using (ZipArchive archive = ZipFile.OpenRead(file))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.ToLower().Contains("update"))
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

        public static void CloseWindow(string windowMainTitle)
        {
            while (IsProgramRunning(windowMainTitle))
            {
                var openWindowProcesses = System.Diagnostics.Process.GetProcesses()
        .Where(p => p.MainWindowHandle != IntPtr.Zero && p.ProcessName != "explorer");

                foreach (var a in openWindowProcesses)
                {
                    //if (a.MainWindowTitle == windowMainTitle)
                    if (a.MainWindowTitle == windowMainTitle)
                    {
                        Output("NKHook launcher is currently open. Close it to continue update");
                        a.CloseMainWindow();
                    }
                }

                Thread.Sleep(350);
            }
        }

        public static bool IsProgramRunning(string windowMainTitle)
        {
            var openWindowProcesses = System.Diagnostics.Process.GetProcesses()
        .Where(p => p.MainWindowHandle != IntPtr.Zero && p.ProcessName != "explorer");

            foreach (var a in openWindowProcesses)
            {
                if (a.MainWindowTitle == windowMainTitle)
                    return true;
            }
            return false;
        }

        public static void Output(string text)
        {
            Console.WriteLine(">> " + text);
        }
    }
}
