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

            TwoConditionalContinuation();
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

        private static void TwoConditionalContinuation()
        {
            Task<int> firstTask = Task.Factory
                     .StartNew<int>(() => { Console.WriteLine("First Task"); return 42; });

            Task secondTask = firstTask.ContinueWith(result => Console.WriteLine("Processed " + result),
                                TaskContinuationOptions.OnlyOnRanToCompletion);

            Task errorHandler = firstTask.ContinueWith(st => Console.WriteLine(st.Exception),
                                            TaskContinuationOptions.OnlyOnFaulted);

            secondTask.Wait();
        }

        #endregion

    }
}