using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TPL
{
    public class ThreadTypeDisplayer
    {

        #region Test Case

        public static void RunTest(List<TaskCreationOptions> options = null)
        {
            if(options != null)
            {
                List<Task> TaskList = new List<Task>();

                foreach (TaskCreationOptions o in options)
                {
                    TaskList.Add(Task.Factory.StartNew(GetThreadType, o));
                }

                foreach (Task t in TaskList)
                {
                    t.Wait();
                }
            }
        }

        #endregion

        #region Custom Methods

        private static void GetThreadType()
        {
            Console.WriteLine("I'm a {0} thread",
                Thread.CurrentThread.IsThreadPoolThread ? "Thread Pool" :
                Thread.CurrentThread.IsBackground ? "Background" : "Custom");
        }

        #endregion

        #region Fields

        //Unneeded for the moment
        //private static int _count = 0;

        #endregion
    }
}
