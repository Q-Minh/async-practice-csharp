using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace TPL
{
    public class AsyncIO
    {

        #region Test Cases

        public static void RunTest()
        {
            Task<string> downloadTask = DownloadWebPageAsync("https://www.google.ca/");

            while (!downloadTask.IsCompleted)
            {
                Console.Write(".");
                Thread.Sleep(250);
            }

            Console.WriteLine(downloadTask.Result);
        }

        #endregion

        #region Custom Methods

        private static Task<string> DownloadWebPageAsync(string url)
        {
            WebRequest request = WebRequest.Create(url);
            IAsyncResult ar = request.BeginGetResponse(null, null);

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
