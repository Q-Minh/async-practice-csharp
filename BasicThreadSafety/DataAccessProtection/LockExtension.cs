using System;
using System.Threading;

namespace DataAccessProtection
{
    public static class LockExtension
    {
        public static Lock Lock(this object obj, TimeSpan timeout)
        {
            bool lockTaken = false;

            try
            {
                Monitor.TryEnter(obj, TimeSpan.FromSeconds(30), ref lockTaken);
                if (lockTaken)
                {
                    return new Lock(obj);
                }
                else
                {
                    throw new TimeoutException("Failed to acquire stateGuard");
                }
            }
            catch
            {
                if (lockTaken)
                {
                    Monitor.Exit(obj);
                }

                throw;
            }
        }
    }

    public struct Lock : IDisposable
    {
        private readonly object _obj;

        public Lock(object obj)
        {
            this._obj = obj;
        }

        public void Dispose()
        {
            Monitor.Exit(this._obj);
        }
    }

}

