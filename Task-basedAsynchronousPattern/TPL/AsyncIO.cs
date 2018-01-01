using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace TPL
{
    public class AsyncIO
    {

        #region Test Cases

        /// <summary>
        /// Requests three websites: a google page, a stackoverflow page, and nfl.com page.
        /// Writes the three html texts in a web-requests.html file created 
        /// or appended to two directories above directory containing executable.
        /// </summary>
        public static void RunTest()
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            Console.WriteLine("Sending web request ");

            Task<string> downloadTask = 
                DownloadWebPageAsync("https://www.google.ca/");
            Task<string> downloadTask2 = 
                DownloadWebPageAsync("https://stackoverflow.com/questions/2099947/simple-description-of-worker-and-i-o-threads-in-net");
            Task<string> downloadTask3 = 
                DownloadWebPageAsync("https://www.nfl.com/");

            while (!downloadTask.IsCompleted && !downloadTask2.IsCompleted && !downloadTask3.IsCompleted) 
            {
                Console.Write(".");
                Thread.Sleep(250);
            }
            Console.WriteLine();

            System.IO.File.WriteAllText("../../web-requests.html", "\n\nGoogle :\n" + downloadTask.Result);
            System.IO.File.AppendAllText("../../web-requests.html", "\n\nStackOverflow question :\n" + downloadTask2.Result);
            System.IO.File.AppendAllText("../../web-requests.html", "\n\nNFL website :\n" + downloadTask3.Result);

            Console.WriteLine("Received and output request in " + s.Elapsed);
        }

        #endregion

        #region Custom Methods

        /// <summary>
        /// This method creates a first thread to begin requesting a web page and
        /// immediately gives control back to the calling thread. Then, it
        /// creates yet another thread at the moment when the results of the 
        /// request are signaled as available. The second Task object encapsulating
        /// the thread that will process the results is returned from this method.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static Task<string> DownloadWebPageAsync(string url)
        {
            WebRequest request = WebRequest.Create(url);

            //Delegate work to an I/O thread that will immediately
            //return execution to calling thread after asking
            //I/O device to perform work and notify when done.
            IAsyncResult ar = request.BeginGetResponse(null, null);

            //Create Task object that encapsulates thread that
            //will be created in future to process results when 
            //I/O device notifies results are available. Is a
            //thread pool thread.
            Task<string> downloadTask =
                Task.Factory
                .FromAsync<string>(ar, iar =>
                {
                    using (WebResponse response = request.EndGetResponse(iar))
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                });

            return downloadTask;
        }

        #endregion
    }
}
