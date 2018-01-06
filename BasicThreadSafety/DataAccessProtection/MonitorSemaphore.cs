using System.Threading;

namespace DataAccessProtection
{
    public class MonitorSemaphore
    {

        #region Constructors

        public MonitorSemaphore(int initialCount, int maxCount)
        {
            this._currentCount = initialCount;
            this._maxCount = maxCount;
        }

        #endregion

        #region Public Methods

        public void Enter()
        {
            lock (_guard)
            {
                while (_currentCount == _maxCount)
                {
                    Monitor.Wait(_guard);
                }
                _currentCount++;
            }
        }

        public void Exit()
        {
            lock (_guard)
            {
                _currentCount--;
                Monitor.Pulse(_guard);
            }
        }

        #endregion

        #region Properties

        public int CurrentCount { get { return _currentCount; } }

        #endregion

        #region Fields

        private int _currentCount;
        private readonly int _maxCount;
        private readonly object _guard = new object();

        #endregion

    }
}
