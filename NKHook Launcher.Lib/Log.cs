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
        public static void Output(string text)
        {
            MessageBox.Show(text);
        }
    }
}
