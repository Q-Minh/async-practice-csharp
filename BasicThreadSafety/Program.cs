using System;
using System.Threading;

namespace BasicThreadSafety
{
    class Program
    {
        static void Main(string[] args)
        {
            RunProducerConsumerTest();
            //ThreadPool.GetAvailableThreads(out int worker, out int io);
            //Console.WriteLine("Number of worker threads : " + worker);
            //Console.WriteLine("Number of io threads : " + io);

            Console.ReadKey();
        }

        private static void RunProducerConsumerTest()
        {
            DataAccessProtection.ProducerConsumer business = new DataAccessProtection.ProducerConsumer(1, 3);
            business.Open();

            while (!(Console.KeyAvailable && (Console.ReadKey(true).Key == ConsoleKey.Q)))
            { Thread.Sleep(500); }

            business.Close();

            Console.ReadKey();
        }
    }
}
