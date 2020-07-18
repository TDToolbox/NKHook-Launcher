using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NKHook_Launcher.Lib
{
    public class FileIO
    {
        /// <summary>
        /// Browse for a file
        /// </summary>
        /// <param name="title">Title of dialog window</param>
        /// <param name="defaultExt">Default extension for dialog window</param>
        /// <param name="filter">File extension filter for dialog window</param>
        /// <param name="startDir">Starting directory for dialog window</param>
        /// <returns></returns>
        public static string BrowseForFile(string title, string defaultExt, string filter, string startDir)
        {
            OpenFileDialog fileDiag = new OpenFileDialog();
            fileDiag.Title = title;
            fileDiag.DefaultExt = defaultExt;
            fileDiag.Filter = filter;
            fileDiag.Multiselect = false;
            fileDiag.InitialDirectory = startDir;

            if (fileDiag.ShowDialog() == DialogResult.OK)
            {
                return fileDiag.FileName;
            }
            else
                return null;
        }
    }
}
