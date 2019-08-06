using System;
using System.Collections.ObjectModel;
using zCompany.Utilities;

namespace zCompany.TaskAide
{
    internal class Session : ISession
    {
        // Fields
        private IDateTimeZone dateTime;
        private ObservableCollection<IInterval> intervals;        

        // Constructors
        public Session(int sid, IDateTimeZone dateTime)
        {
            this.sid = sid;
            this.dateTime = dateTime;

            this.intervals = new ObservableCollection<IInterval>();
            this.Intervals = new ReadOnlyObservableCollection<IInterval>(this.intervals);
        }

        // Properties
        public IInterval ActiveInterval { get; set; }

        public IDateTimeZone DateTimeStart
        {
            get { return this.dateTime; }
        }

        public ReadOnlyObservableCollection<IInterval> Intervals { get; private set; }

        private int sid;
        public int SID
        {
            get => this.sid;
        }

        private TimeSpan span;
        public TimeSpan Span
        {
            get { return this.span; }
            set
            {
                this.span = value;
            }
        }

        public TimeSpan StartTime
        {
            get => this.dateTime.LocalTime;
        }

        // Methods
        public void Pop()
        {
            this.intervals.Remove(this.ActiveInterval);
        }

        public void Push(Interval interval)
        {
            this.intervals.Add(interval);
        }
    }
}
