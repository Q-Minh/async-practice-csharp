using System;
using System.IO;
using System.Net;
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

            Task<string> ioTask = SimpleContinuationIO("https://www.google.ca/");
            Console.WriteLine(ioTask.Result);
        }

        public static void RunNestedTaskTest()
        {

        }

        #endregion

        #region Custom Methods

        //Not very useful because this is compute-based. We could achieve the same
        //functionality by running these tasks in the same function.
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

            Task secondTask = firstTask.ContinueWith(ft => Console.WriteLine("Processed " + ft.Result),
                                TaskContinuationOptions.OnlyOnRanToCompletion);

            Task errorHandler = firstTask.ContinueWith(st => Console.WriteLine(st.Exception),
                                            TaskContinuationOptions.OnlyOnFaulted);

            secondTask.Wait();
        }

        //I/O based simple continuation
        private static Task<string> SimpleContinuationIO(string url)
        {
            WebRequest request = WebRequest.Create(url);
            Task<WebResponse> response = request.GetResponseAsync();

            return response
                 .ContinueWith<string>(grt =>
                 {
                     using (var reader = new StreamReader(grt.Result.GetResponseStream())) {
                         return reader.ReadToEnd();
                     }
                 });
        }

        #endregion

    }
}