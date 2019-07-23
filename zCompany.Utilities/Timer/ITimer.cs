using System;

namespace zCompany.Utilities
{
    public interface ITimer
    {
        // Delegates
        Action SecondElapsed { set; }

        // Properties
        TimeSpan SecondsElapsed { get; }

        // Methods
        void Start();

        void Stop();
    }
}
