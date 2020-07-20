using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NKHook_Launcher.Lib;

namespace NKHook_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Log.Instance.MessageLogged += Instance_MessageLogged;

            Log.Output("Welcome to NKHook5 Launcher!");

            if (NKHook.DoesNkhExist())
                DownloadNKH_Button.Content = "  Re-download NKHook  ";


            UpdateHandler update = new UpdateHandler
            {
                GitApiReleasesURL = "https://api.github.com/repos/TDToolbox/NKHook-Launcher/releases",
                ProjectName = "NKHook5 Launcher",
                ProjectExePath = Environment.CurrentDirectory + "\\NKHook Launcher.exe",
                UpdaterExeName = "Updater for NKHook Launcher.exe",
                UpdatedZipName = "NKHook Launcher.zip"
            };

            Thread thread = new Thread(update.HandleUpdates);
            BgThread.AddToQueue(thread);
        }

        private void Instance_MessageLogged(object sender, Log.LogEvents e)
        {
            Console.Dispatcher.BeginInvoke((Action)(() =>
            {
                Console.AppendText(e.Message);
                Console.ScrollToEnd();
            }));            
        }

        private void OpenNkhDir_Button_Click(object sender, RoutedEventArgs e)
        {
            Log.Output("Opening NKHook5 directory...");
            NKHook.OpenNkhDir();
        }

        private void OpenBTD5Dir_Button_Click(object sender, RoutedEventArgs e)
        {
            string gameDir = SteamUtils.GetGameDir(GameType.BTD5);
            SteamUtils.GetGameDir(GameType.BTD5);
            if (!Guard.IsStringValid(gameDir))
            {
                Log.Output("Error! Failed to find BTD5 Directory");
                return;
            }

            Log.Output("Opening BTD5 directory");
            Process.Start(gameDir);
        }

        private void BrowseForPlugin_Button_Click(object sender, RoutedEventArgs e)
        {
            Log.Output("Please select the plugins you want to add...");
            var result = FileIO.BrowseForFile("Browse for NKHook plugins", "chai", "Chai files (*.chai)|*.chai|All files (*.*)|*.*", "");
            if (!Guard.IsStringValid(result))
            {
                Log.Output("Error! Either no plugin was selected or the one you selected was invalid");
                return;
            }

            FileInfo f = new FileInfo(result);
            f.CopyTo(NKHook.nkhDir + "\\Plugins\\" + f.Name, true);
        }

        private void RunNKH_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!BgThread.IsThreadRunning())
            {
                Log.Output("Launching NKHook5");
                NKHook.LaunchNKH();
            }
            else
                MessageBox.Show("Can't run the game, currently doing something. " +
                    "Please wait about 30 seconds and try again");
        }

        private void DownloadNKH_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!BgThread.IsThreadRunning())
            {
                Log.Output("Downloading NKHook5");
                var thread = new Thread(() => NKHook.DownloadNKH());
                BgThread.AddToQueue(thread);
            }
            else
                MessageBox.Show("Can't run the game, currently doing something. " +
                    "Please wait about 30 seconds and try again");
        }
    }
}
