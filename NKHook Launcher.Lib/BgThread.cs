﻿using System;
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
        #region Events
        public event EventHandler<ThreadingEventArgs> ThreadQueueFinished;
        public event EventHandler<ThreadingEventArgs> ThreadQueueStarted;
        public event EventHandler<ThreadingEventArgs> ThreadQueueItemAdded;
        public event EventHandler<ThreadingEventArgs> ThreadQueueItemRemoved;

        public class ThreadingEventArgs : EventArgs
        {
            public Queue<Thread> ThreadQueue { get; set; }
            public Thread Thread { get; set; }
        }

        /// <summary>
        /// Event fired when a thread has been added to the ThreadQueue
        /// </summary>
        /// <param name="e">Passes current Queue as argument</param>
        public void OnThreadQueueStarted(ThreadingEventArgs e)
        {
            EventHandler<ThreadingEventArgs> handler = ThreadQueueStarted;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Event fired when ThreadQueue finished executing all threads
        /// </summary>
        /// <param name="e">Can be null</param>
        public void OnThreadQueueFinished(ThreadingEventArgs e)
        {
            EventHandler<ThreadingEventArgs> handler = ThreadQueueFinished;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Event fired when an item is added to the ThreadQueue
        /// </summary>
        /// <param name="e">The thread that was added to the queue is passed as an arg</param>
        public void ItemAddedToThreadQueue(ThreadingEventArgs e)
        {
            EventHandler<ThreadingEventArgs> handler = ThreadQueueItemAdded;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Event fired when an item is removed from ThreadQueue
        /// </summary>
        /// <param name="e">The thread that was removed from queue is passed as arg</param>
        public void ItemRemovedFromThreadQueue(ThreadingEventArgs e)
        {
            EventHandler<ThreadingEventArgs> handler = ThreadQueueItemRemoved;
            if (handler != null)
                handler(this, e);
        }
        #endregion


        #region Properties

        private static BgThread instance;

        /// <summary>
        /// Singleton instance of this class
        /// </summary>
        public static BgThread Instance
        {
            get
            {
                if (instance == null)
                    instance = new BgThread();
                return instance;
            }
        }

        /// <summary>
        /// The thread this class uses to do threading
        /// </summary>
        public static Thread ThreadInstance { get; private set; }

        /// <summary>
        /// A queue of threads that need to be ran
        /// </summary>
        public static Queue<Thread> ThreadQueue { get; private set; }
        #endregion


        /// <summary>
        /// Check if the background thread is in use or not
        /// </summary>
        /// <returns>Whether or not the background thread is in use</returns>
        public static bool IsThreadRunning()
        {
            if (ThreadInstance == null || !ThreadInstance.IsAlive)
                return false;

            return true;
        }

        /*public static void AddToQueue(Func func)
        {
            AddToQueue(new Thread(() => func()));
        }*/

        /// <summary>
        /// Add a thread to the ThreadQueue. It will execute the thread immediately if the thread Instance for this class isn't running
        /// </summary>
        /// <param name="thread">Thread to be added to the ThreadQueue</param>
        public static void AddToQueue(Thread thread)
        {
            if (ThreadQueue == null)
                ThreadQueue = new Queue<Thread>();

            ThreadQueue.Enqueue(thread);

            ThreadingEventArgs args = new ThreadingEventArgs();
            args.Thread = thread;
            Instance.ItemAddedToThreadQueue(args);

            if (!IsThreadRunning())
                Instance.ExcecuteQueue();
        }

        /// <summary>
        /// Add a list of threads to the ThreadQueue. It will execute them immediately if the thread Instance for this class isn't running
        /// </summary>
        /// <param name="threads"></param>
        public static void AddToQueue(List<Thread> threads)
        {
            if (threads == null || threads.Count() <= 0)
            {
                Log.Output("Error! Tried adding an empty thread list to the Thread Queue");
                return;
            }

            foreach (var thread in threads)
                AddToQueue(thread);
        }

        /// <summary>
        /// Excecutes threads on queue until none are left
        /// </summary>
        private void ExcecuteQueue()
        {
            if (ThreadQueue.Count() <= 0)
                return;

            ThreadInstance = new Thread(() =>
            {
                ThreadingEventArgs args = new ThreadingEventArgs();
                args.ThreadQueue = ThreadQueue;
                OnThreadQueueStarted(args);

                while (ThreadQueue.Count > 0)
                {
                    ThreadQueue.Peek().IsBackground = true;
                    ThreadQueue.Peek().Start();
                    ThreadQueue.Peek().Join();

                    ThreadingEventArgs removeArgs = new ThreadingEventArgs();
                    args.Thread = ThreadQueue.Peek();

                    ThreadQueue.Dequeue();
                    ItemRemovedFromThreadQueue(args);
                }

                OnThreadQueueFinished(null);
            });

            ThreadInstance.IsBackground = true;
            ThreadInstance.Start();
        }

        /// <summary>
        /// Get the background thread instance of the class
        /// </summary>
        /// <returns>the background thread instance</returns>
        public static Thread GetThreadInst() => ThreadInstance;

        /// <summary>
        /// Get the instance of BgThread class
        /// </summary>
        /// <returns></returns>
        public static BgThread GetInstance() => Instance;
    }
}
