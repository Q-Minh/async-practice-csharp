using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPL
{
    public class DataImporter
    {

        #region Test Case

        public static void RunTest()
        {
            //DataImporter d = new DataImporter();
            string dir = "C:/Users/Minh";
            string dir2 = "C:/Program Files";
            string dir3 = "C:/User/Minh/Documents/C#";
            
            //If Task.Wait() is called directly after the Factory.StartNew(),
            //there is no asynchrony since program execution will wait for
            //the task to end without having processed any other statement
            //in between.
            Task t1 = Task.Factory.StartNew(() => Import(dir))/*.Wait()*/;
            Task t2 = Task.Factory.StartNew(() => Import(dir2));
            Task t3 = Task.Factory.StartNew(() => Import(dir3));

            //Let's try with different callback instances instead of Import()
            //Task t4 = Task.Factory.StartNew(() => Console.WriteLine(dir + " : " + _count++));
            //Task t5 = Task.Factory.StartNew(() => Console.WriteLine(dir2 + " : " + _count++));
            //Task t6 = Task.Factory.StartNew(() => Console.WriteLine(dir3 + " : " + _count++));

            t1.Wait();
            t2.Wait();
            t3.Wait();
            //t4.Wait();
            //t5.Wait();
            //t6.Wait();
        }

        #endregion

        #region Custom methods

        private static void Import(string directory)
        {
            // Import files from this.directory
            Console.WriteLine(directory + " : " + _count++);
        }

        #endregion

        #region fields

        private static int _count = 0;

        #endregion
    }
    
}
