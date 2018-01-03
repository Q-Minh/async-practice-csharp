using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace TPL
{
    public class CancelTask
    {
        #region Test Cases

        public static void RunTest()
        {
            PrintWithCancellationAsync();
        }

        #endregion

        #region Custom Methods

        private static void PrintWithCancellationAsync()
        {
            CancellationTokenSource tSource = new CancellationTokenSource();
            CancellationToken token = tSource.Token;

            /*If relative paths encapsulate directory or file names that have special characters,
            like "-" hyphen in my case, you should use absolute path or else DirectyNotFoundException
            will be thrown(or file version of exception).
            Task importTask =
            DataImportAsync(@"C:\Users\Minh\source\repos\AsynchronousProgrammingPractice\Task-basedAsynchronousPattern\TPL\", token);*/

            Task importTask = DataImportAsync(@"..\..\TPL\", token);
            
            //For some reason, the importTask.IsCanceled is never true
            //Even if ThrowIfCancellationRequested() is called in this case,
            //the TaskStatus is IsCompleted.
            while (!importTask.IsCompleted && !importTask.IsCanceled) {
                Console.Write(".");
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                    tSource.Cancel();

                Thread.Sleep(100);
            }

            Console.WriteLine(importTask.IsCanceled);
            Console.WriteLine(importTask.Status);
        }

        private static Task DataImportAsync(string directoryname, CancellationToken token)
        {
            return Task.Factory.StartNew(() => DataImport(directoryname, token), token);
        }

        private static void DataImport(string directoryname, CancellationToken token)
        {
            //If the cancellation is outside of the for loop, it will never be thrown
            //after the loop has started.
            //token.ThrowIfCancellationRequested();

            try {
                for (int i = 0; i < 1000; ++i) {

                    token.ThrowIfCancellationRequested();

                    foreach (FileInfo file in new DirectoryInfo(directoryname).GetFiles("*.cs")) {
                        Console.WriteLine(File.ReadAllText(file.FullName));
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (OperationCanceledException e) {
                Console.WriteLine(String.Concat("\n", e.Message, "\nInner exception : ",
                    e.InnerException == null ? "No inner exception." : e.InnerException.Message));
                Console.WriteLine(String.Concat("Data processing has stopped midway before any next file could open",
                                    " and after any previous file was closed."));

                //If the exception is not rethrown, the Task will not be in the IsCanceled state.
                throw;
            }
        }

        #endregion
    }
}
