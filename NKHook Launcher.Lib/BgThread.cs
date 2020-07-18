using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NKHook_Launcher.Lib
{
    /// <summary>
    /// Background thread class. Used to manage threading in the application
    /// </summary>
    public class BgThread
    {
        #region Properties
        private static BgThread bg;
        private static BgThread bgThread
        {
            get
            {
                if (bg == null)
                    bg = new BgThread();
                return bg;
            }
        }

        /// <summary>
        /// System.Thread instance for this class. It is a singleton
        /// </summary>
        private static Thread Instance { get; set; }

        #endregion

        /// <summary>
        /// Check if the background thread is in use or not
        /// </summary>
        /// <returns>Whether or not the background thread is in use</returns>
        public static bool IsRunning()
        {
            if (Instance == null || !Instance.IsAlive)
                return false;

            return true;
        }

        /// <summary>
        /// Run a method on the thread using a ThreadStart
        /// </summary>
        /// <param name="start">method to run on thread</param>
        public static void Run(ThreadStart start)
        {
            var thread = new Thread(start);
            Run(thread);
        }


        /// <summary>
        /// Run a method on the thread using a Thread
        /// </summary>
        /// <param name="thread">The thread to run on the background thread</param>
        public static void Run(Thread thread)
        {
            Instance = thread;
            Instance.IsBackground = true;
            Instance.Start();
        }

        /// <summary>
        /// Get the background thread instance of the class
        /// </summary>
        /// <returns>the background thread instance</returns>
        public static Thread GetInstance()
        {
            return Instance;
        }
    }
}
