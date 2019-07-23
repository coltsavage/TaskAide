using System;
using System.Collections.Generic;
using zCompany.Utilities;

namespace zCompany.TaskAide.Tests
{
    public class AppSimulationDatabaseFixture : IDisposable
    {
        // Constructors
        public AppSimulationDatabaseFixture()
        {
            this.Db = new Database("Filename=AppSimulationTestDb.db");

            this.Tasks = new List<Task>();
            this.Tasks.Add(new Task(1, "alpha"));
            this.Tasks.Add(new Task(2, "beta"));
            this.Tasks.Add(new Task(3, "gamma"));

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dateTimeLocal = new DateTimeOffset(2019, 7, 13, 8, 22, 35, timeZone.BaseUtcOffset);
            var workDateTime = new DateTimeZone(dateTimeLocal.ToUniversalTime(), timeZone);

            this.Sessions = new List<Session>();
            this.Sessions.Add(new Session(1, workDateTime));

            this.Intervals = new List<Interval>();
            this.Intervals.Add(new Interval(1, this.Tasks[0].TID, this.Sessions[0].SID));
            this.Intervals.Add(new Interval(2, this.Tasks[1].TID, this.Sessions[0].SID));
            this.Intervals.Add(new Interval(3, this.Tasks[2].TID, this.Sessions[0].SID));
            this.Intervals.Add(new Interval(4, this.Tasks[0].TID, this.Sessions[0].SID));
            this.Intervals.Add(new Interval(5, this.Tasks[1].TID, this.Sessions[0].SID));
            this.Intervals.Add(new Interval(6, this.Tasks[0].TID, this.Sessions[0].SID));

            for (int i = 1; i < this.Intervals.Count; i++)
            {
                this.Intervals[i - 1].Successor = this.Intervals[i];
                this.Intervals[i].Predecessor = this.Intervals[i - 1];
            }

            TimeSpan start = TimeSpan.Zero;
            int span = 60;
            foreach (var interval in this.Intervals)
            {
                interval.Start = start;
                interval.Span = new TimeSpan(0, span, 0);
                start += interval.Span;
            }

            var latestTime = dateTimeLocal.ToUniversalTime().Add(start);
            this.SystemTime = new SystemTimeDev(latestTime, timeZone);
            this.Timer = new TimerDev(this.SystemTime);
        }

        // Destructors
        public void Dispose()
        {
            
        }

        // Properties
        internal Database Db { get; private set; }

        internal List<Interval> Intervals { get; private set; }

        internal List<Session> Sessions { get; private set; }

        internal List<Task> Tasks { get; private set; }

        internal SystemTimeDev SystemTime { get; private set; }

        internal TimerDev Timer { get; private set; }

        // Methods
        internal void DestroyTable(string tableName)
        {
            this.Db.DestroyTable(tableName);
        }

        internal void PopulateIntervals(string tableName)
        {
            var table = new IntervalTable(tableName, this.Db);
            foreach (var interval in this.Intervals)
            {
                table.Add(interval);
            }
        }

        internal void PopulateSessions(string tableName)
        {
            var table = new SessionTable(tableName, this.Db);
            foreach (var session in this.Sessions)
            {
                table.Add(session);
            }
        }

        internal void PopulateTasks(string tableName)
        {
            var table = new TaskTable(tableName, this.Db);
            foreach (var task in this.Tasks)
            {
                table.Add(task);
            }
        }
    }
}
