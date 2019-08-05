using System;

namespace zCompany.Windows.Charts
{
    public class IntervalResizedEventArgs
    {
        // Constructors
        public IntervalResizedEventArgs(IIntervalViewModel viewModel, int startDelta, int spanDelta)
        {
            this.ViewModel = viewModel;
            this.StartDelta = startDelta;
            this.SpanDelta = spanDelta;
        }

        // Properties
        public int SpanDelta { get; }

        public int StartDelta { get; }

        public IIntervalViewModel ViewModel { get; }
    }
}
