using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace TPL
{
    public class Computation
    {

        #region Test Cases
        
        public static void RunCombinationsTestAsync(uint numberOfOptions, uint numberOfChoices, TaskCreationOptions hint = TaskCreationOptions.None)
        {
            try {
                ComputeCombinationAsync(numberOfOptions, numberOfChoices, hint);
            }
            catch (AggregateException a) {
                foreach (Exception e in a.Flatten().InnerExceptions)
                    Console.WriteLine(String.Concat(e.GetType().Name, " : ", e.Message));
            }
        }

        public static void RunCombinationsTest(uint numberOfOptions, uint numberOfChoices)
        {
            ComputeCombination(numberOfOptions, numberOfChoices);
        }

        #endregion

        #region Custom Methods

        /// <summary>
        /// Asynchronously computes and prints number of possible combinations of <paramref name="numberOfChoices"/> 
        /// choices in a given set of <paramref name="numberOfOptions"/> options
        /// without repetition and where order is insignificant.
        /// The <paramref name="numberOfOptions"/> and <paramref name="numberOfChoices"/> parameters'
        /// values can only range from 0 to 4,294,967,295 as they are unsigned integers.
        /// </summary>
        /// <param name="numberOfOptions"></param>
        /// <param name="numberOfChoices"></param>
        /// <exception cref="AggregateException"></exception>
        private static void ComputeCombinationAsync(uint numberOfOptions, 
            uint numberOfChoices, TaskCreationOptions hint)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            Task<BigInteger> num = 
                Task.Factory.StartNew(() => Factorial(numberOfOptions), hint);
            Task<BigInteger> discardInvalidChoices = 
                Task.Factory.StartNew(() => Factorial(numberOfOptions - numberOfChoices), hint);
            Task<BigInteger> discardOrder = 
                Task.Factory.StartNew(() => Factorial(numberOfChoices), hint);

            BigInteger numberOfPossibleCombinations = 
                num.Result / (discardInvalidChoices.Result * discardOrder.Result);

            s.Stop();

            Console.WriteLine(numberOfPossibleCombinations);
            Console.WriteLine("Calculated in : " + s.Elapsed);
        }

        /// <summary>
        /// Synchronously computes and prints number of possible combinations of <paramref name="numberOfChoices"/> 
        /// choices in a given set of <paramref name="numberOfOptions"/> options
        /// without repetition and where order is insignificant.
        /// The <paramref name="numberOfOptions"/> and <paramref name="numberOfChoices"/> parameters'
        /// values can only range from 0 to 4,294,967,295 as they are unsigned integers.
        /// </summary>
        /// <param name="numberOfOptions"></param>
        /// <param name="numberOfChoices"></param>
        private static void ComputeCombination(uint numberOfOptions, uint numberOfChoices)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            BigInteger num = Factorial(numberOfOptions);
            BigInteger discardInvalidChoices = Factorial(numberOfOptions - numberOfChoices);
            BigInteger discardOrder = Factorial(numberOfChoices);

            BigInteger numberOfPossibleCombinations = num / (discardInvalidChoices * discardOrder);

            s.Stop();

            Console.WriteLine(numberOfPossibleCombinations);
            Console.WriteLine("Calculated in : " + s.Elapsed);
        }

        public static BigInteger Factorial(uint n)
        {
            if (n == 1 || n == 0)
                return 1;
            if (n == 2)
                return 2;

            BigInteger b = new BigInteger(n);

            for (uint i = (n - 1); i > 1; --i)
            {
                b *= i;
            }

            return b;
        }

        #endregion

    }
}
