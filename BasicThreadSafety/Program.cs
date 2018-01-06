using System;


namespace BasicThreadSafety
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccessProtection.ProducerConsumer business = new DataAccessProtection.ProducerConsumer(1, 3);
            business.Open();

            while (!(Console.KeyAvailable && (Console.ReadKey(true).Key == ConsoleKey.Q)))
            {}

            business.Close();

            Console.ReadKey();
        }
    }
}
