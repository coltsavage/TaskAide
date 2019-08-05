using System;

namespace zCompany.TaskAide.WindowsApp
{
    internal class UserChangedIntervalEventArgs
    {
        // Constructors
        public UserChangedIntervalEventArgs(IInterval interval, TimeSpan startDelta, TimeSpan spanDelta)
        {
            this.Interval = interval;
            this.StartDelta = startDelta;
            this.SpanDelta = spanDelta;
        }

        // Properties
        public IInterval Interval { get; }

        public TimeSpan SpanDelta { get; }

        public TimeSpan StartDelta { get; }
    }
}
