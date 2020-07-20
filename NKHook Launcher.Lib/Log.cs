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
        #region Properties
        private static Log instance;

        /// <summary>
        /// Singleton instance of this class
        /// </summary>
        public static Log Instance
        {
            get
            {
                if (instance == null)
                    instance = new Log();

                return instance;
            }
        }
        #endregion

        #region Events
        public event EventHandler<LogEvents> MessageLogged;

        public class LogEvents : EventArgs
        {
            public string Message { get; set; }
        }

        /// <summary>
        /// When a message has been sent to the Output() function
        /// </summary>
        /// <param name="e">LogEvent args containing the output message</param>
        public void OnMessageLogged(LogEvents e)
        {
            EventHandler<LogEvents> handler = MessageLogged;
            if (handler != null)
                handler(this, e);
        }

        #endregion


        /// <summary>
        /// Passes message to OnMessageLogged for Event Handling.
        /// </summary>
        /// <param name="text">Message to output to user</param>
        public static void Output(string text)
        {
            LogEvents args = new LogEvents();
            args.Message = ">> " + text + Environment.NewLine;
            Instance.OnMessageLogged(args);
        }
    }
}
