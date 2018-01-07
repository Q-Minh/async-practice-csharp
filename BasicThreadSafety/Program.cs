using System;
using System.Threading;

namespace BasicThreadSafety
{
    class Program
    {
        static void Main(string[] args)
        {
            RunProducerConsumerTest();
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
