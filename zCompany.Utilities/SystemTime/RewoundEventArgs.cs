using System;

namespace zCompany.Utilities
{
    public class RewoundEventArgs
    {
        // Constructors
        public RewoundEventArgs(TimeSpan amount)
        {
            this.Amount = amount;
        }

        // Properties
        public TimeSpan Amount { get; }
    }
}
