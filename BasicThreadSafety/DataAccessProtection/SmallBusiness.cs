using System;

namespace DataAccessProtection
{
    public class SmallBusiness
    {

        #region Constructor

        public SmallBusiness(decimal cash, decimal receivables)
        {
            this._cash = cash;
            this._receivables = receivables;
        }

        #endregion

        #region Methods

        public void ReceivePayment(decimal amount)
        {
            using (_guard.Lock(TimeSpan.FromSeconds(30)))
            {
                this._cash += amount;
                this._receivables -= amount;
            }
        }

        #endregion

        #region Properties

        public decimal NetWorth
        {
            get
            {
                using (_guard.Lock(TimeSpan.FromSeconds(5)))
                {
                    return _cash + _receivables;
                }
            }
        }

        #endregion

        #region Fields

        private decimal _cash;
        private decimal _receivables;
        private readonly object _guard = new object();

        #endregion

    }
}
