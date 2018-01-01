using System;

namespace TaskbasedAsynchronousPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            //TPL.DataImporter.RunTest();

            //TPL.ThreadTypeDisplayer.RunTest(new List<TaskCreationOptions>()
            //{
            //    TaskCreationOptions.LongRunning,
            //    TaskCreationOptions.None,
            //    TaskCreationOptions.LongRunning,
            //    TaskCreationOptions.None,
            //    TaskCreationOptions.LongRunning,
            //    TaskCreationOptions.None
            //});

            //Long running option uses a background thread. I think it is slower than
            ////foreground thread.
            //TPL.Computation.RunCombinationsTestAsync(49000, 600/*, TaskCreationOptions.LongRunning*/);
            //TPL.Computation.RunCombinationsTest(49000, 600);

            TPL.AsyncIO.RunTest();

            Console.WriteLine("All Done");
            Console.ReadKey();
        }

    }
}
