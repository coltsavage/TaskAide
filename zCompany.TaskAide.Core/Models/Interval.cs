using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace zCompany.TaskAide
{
    internal class Interval : IInterval
    {        
        // Constructors
        public Interval(int iid, int taskId, int sessionId)
        {
            this.iid = iid;
            this.TaskId = taskId;
            this.sessionId = sessionId;
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        private int iid;
        public int IID
        {
            get => this.iid;
        }

        public Interval Predecessor { get; set; }

        private int sessionId;
        public int SessionId
        {
            get => this.sessionId;
        }

        private TimeSpan span;
        public TimeSpan Span
        {
            get { return this.span; }
            set
            {
                this.span = value;
                this.RaisePropertyChanged();
            }
        }

        private TimeSpan start;
        public TimeSpan Start
        {
            get { return this.start; }
            set
            {
                this.start = value;
                this.RaisePropertyChanged();
            }
        }

        public Interval Successor { get; set; }

        public int TaskId { get; set; }

        // Methods
        public void UserChanged(TimeSpan startDelta, TimeSpan spanDelta)
        {
            if (startDelta != TimeSpan.Zero)
            {
                this.Start += startDelta;
                this.Span -= startDelta;
                if (this.Predecessor != null)
                {
                    this.Predecessor.Span += startDelta;
                }
            }
            if (spanDelta != TimeSpan.Zero)
            {
                this.Span += spanDelta;
                if (this.Successor != null)
                {
                    this.Successor.Start += spanDelta;
                    this.Successor.Span -= spanDelta;
                }
            }
        }

        public void SecondsIncrement()
        {
            this.Span += new TimeSpan(0, 0, 1);
        }

        // Helpers
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
