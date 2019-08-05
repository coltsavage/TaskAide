using System;
using System.Collections.Generic;
using zCompany.Utilities;

namespace zCompany.TaskAide
{
    internal class Session : ISession
    {
        // Fields
        private IDateTimeZone dateTime;

        // Constructors
        public Session(int sid)
        {
            this.sid = sid;
            this.Intervals = new List<Interval>();
        }
        public Session(int sid, IDateTimeZone dateTime)
        {
            this.sid = sid;
            this.dateTime = dateTime;
            this.Intervals = new List<Interval>();
        }

        // Properties
        public IInterval ActiveInterval { get; set; }

        public IDateTimeZone DateTimeStart
        {
            get { return this.dateTime; }
        }

        private List<Interval> intervals;
        public List<Interval> Intervals
        {
            get { return this.intervals; }
            set
            {
                this.intervals = value;
            }
        }

        public bool IsNew
        {
            get => dateTime == null;
        }

        public bool IsRunning { get; set; }

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
        public void Add(Interval interval)
        {
            this.Intervals.Add(interval);
        }

        public void Resume(IDateTimeZone dateTime)
        {

        }

        public void Start(IDateTimeZone dateTime)
        {
            this.dateTime = dateTime;
        }

        public void Stop()
        {

        }
    }
}
