using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            //foreground thread.
            TPL.Computation.RunCombinationsTestAsync(60000, 600/*, TaskCreationOptions.LongRunning*/);
            TPL.Computation.RunCombinationsTest(60000, 600);

            Console.WriteLine("All Done");
            Console.ReadKey();
        }

    }
}
