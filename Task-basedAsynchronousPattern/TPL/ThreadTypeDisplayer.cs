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
            try { 
                if (options != null)
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
            catch(AggregateException a) {
                foreach (Exception e in a.Flatten().InnerExceptions)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch(ObjectDisposedException o) {
                Console.WriteLine(String.Concat(o.Message, "\nInner exception : ", o.InnerException.Message));
            }
        }

        #endregion

        #region Custom Methods

        public static void GetThreadType()
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
