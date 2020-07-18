using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

            if (NKHook.DoesNkhExist())
                DownloadNKH_Button.Content = "  Re-download NKHook  ";

            if (!BgThread.IsRunning())
            {
                Thread thread = new Thread(UpdateHandler.HandleUpdates);
                BgThread.Run(thread);
            }
        }

        private void OpenNkhDir_Button_Click(object sender, RoutedEventArgs e)
        {
            NKHook.OpenNkhDir();
        }

        private void OpenBTD5Dir_Button_Click(object sender, RoutedEventArgs e)
        {
            string gameDir = SteamUtils.GetGameDir(Game.BTD5);
            SteamUtils.GetGameDir(Game.BTD5);
            if (!Guard.IsStringValid(gameDir))
            {
                Log.Output("Error! Failed to find BTD5 Directory");
                return;
            }

            Process.Start(gameDir);
        }

        private void BrowseForPlugin_Button_Click(object sender, RoutedEventArgs e)
        {
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
            if (!BgThread.IsRunning())
                NKHook.LaunchNKH();
            else
                MessageBox.Show("Can't run the game, currently doing something. " +
                    "Please wait about 30 seconds and try again");
        }

        private void DownloadNKH_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!BgThread.IsRunning())
            {
                var thread = new System.Threading.Thread(() => NKHook.DownloadNKH());
                BgThread.Run(thread);
            }
            else
                MessageBox.Show("Can't run the game, currently doing something. " +
                    "Please wait about 30 seconds and try again");
        }
    }
}
