using System;
using System.Threading.Tasks;

namespace TPL
{
    public class TaskRelationships
    {
        #region Test Cases

        public static void RunChainedTaskTest()
        {
            SimpleContinuation();
        }

        public static void RunNestedTaskTest()
        {

        }

        #endregion

        #region Custom Methods

        private static void SimpleContinuation()
        {
            Task<int> firstTask = Task.Factory
                      .StartNew<int>(() => { Console.WriteLine("First Task"); return 42; });

            Task secondTask = firstTask
                 .ContinueWith(ft => Console.WriteLine("Second Task, First task returned {0}", ft.Result));

            secondTask.Wait();
        }

        #endregion

    }
}
