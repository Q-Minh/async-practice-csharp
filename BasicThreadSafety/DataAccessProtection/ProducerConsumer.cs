using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessProtection
{
    public class ProducerConsumer
    {

        #region Constructors

        public ProducerConsumer(byte nproducers, byte nconsumers)
        {
            this._nproducers = nproducers;
            this._nconsumers = nconsumers;
        }

        #endregion

        #region Public Methods

        public void Open()
        {

            CancellationToken ctoken = _manager.Token;

            for (byte i = 0; i < _nproducers; ++i)
            {
                Task.Run(() =>
                {
                    try
                    {
                        Produce(_workload, ctoken);
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Production closed");
                    }
                }, ctoken);
            }

            for (byte i = 0; i < _nconsumers; ++i)
            {
                Task.Run(() =>
                {
                    try
                    {
                        Consume(_workload, ctoken);
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Client finished");
                    }
                }, ctoken);
            }
        }

        public void Close()
        {
            _manager.Cancel();
        }

        #endregion

        #region Private Methods

        private void Produce(object obj, CancellationToken ctoken)
        {
            var queue = (Queue<int>)obj;
            var rnd = new Random();

            while (true)
            {
                lock (queue)
                {
                    if (ctoken.IsCancellationRequested)
                        Monitor.PulseAll(queue);
                    ctoken.ThrowIfCancellationRequested();

                    queue.Enqueue(rnd.Next(100));

                    Monitor.Pulse(queue);
                }

                Thread.Sleep(rnd.Next(2000));
            }
        }

        private void Consume(object obj, CancellationToken ctoken)
        {
            var queue = (Queue<int>)obj;
            int val;

            while (true)
            {
                lock (queue)
                {
                    //Theoretically, the Producer could reacquire the monitor before
                    //this thread has been rescheduled. In that case, Pulse(queue)
                    //would be called twice and thus wake up two threads. Therefore,
                    //we write a while loop so that when the thread wakes up and continues
                    //execution after the Wait() statement, it will recheck the queue count
                    //and not automatically dequeue right away.
                    while (queue.Count == 0)
                    {
                        Monitor.Wait(queue);
                        ctoken.ThrowIfCancellationRequested();
                    }

                    val = queue.Dequeue();
                }

                ProcessValue(val);
            }
        }

        private static void ProcessValue(int val)
        {
            //Do something with val
            Console.WriteLine("Consumed : " + val);
        }

        #endregion

        #region Fields

        private Queue<int> _workload = new Queue<int>();
        private CancellationTokenSource _manager = new CancellationTokenSource();
        private byte _nproducers;
        private byte _nconsumers;

        #endregion

    }
}
