using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsynchronousProgrammingPractice
{
    class Program
    {
        public static int count = 0;

        delegate void ProcessTextInput(string input);

        static void Main(string[] args)
        {
            ProcessTextInput textProcessor = new ProcessTextInput(input =>
            {
                ++count;
                Console.WriteLine(input + count);
            });

            OnHandRaised(textProcessor);
            OnHandRaised(textProcessor);
            OnHandRaised(textProcessor);
            OnHandRaised(textProcessor);
            OnHandRaised(textProcessor);

            Console.ReadKey();
        }

        private static void PerformSpeak(IAsyncResult iar)
        {
            ProcessTextInput speak = (ProcessTextInput)iar.AsyncState;

            try
            {
                speak.EndInvoke(iar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void OnHandRaised(ProcessTextInput AsyncCallback)
        {
            AsyncCallback.BeginInvoke("Afsaneh", new AsyncCallback(PerformSpeak), AsyncCallback);
        }
    }
}
