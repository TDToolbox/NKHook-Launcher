using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace NKHook_Launcher.Lib
{
    public class Log
    {
        /// <summary>
        /// Show message to user. This should be switched to a rich text box
        /// </summary>
        /// <param name="text">Text to show user</param>
        public static void Output(string text)
        {
            MessageBox.Show(text);
        }
    }
}
