using System;
using Windows.Storage;
using System.Collections.Generic;
using zCompany.Utilities;

namespace zCompany.TaskAide
{
    public class TaskAide
    {
        // Fields
        //private IntervalTable intervalTable;
        //private UidGenerator intervalUidGenerator;
        private TaskList taskList;
        private UidGenerator taskUidGenerator;
        private Session session;
        //private SessionTable sessionTable;
        //private UidGenerator sessionUidGenerator;
        private ISystemTime systemTime;
        private ITimer timer;
        
        // Constructors
        public TaskAide(Database db, ISystemTime systemTime, ITimer timer)
        {
            this.systemTime = systemTime;
            this.timer = timer;

            var taskTable = new TaskTable("Tasks", db);
            this.taskUidGenerator = new UidGenerator(
                ApplicationData.Current.RoamingSettings,
                "TaskUids",
                (uid) => { return taskTable.Get(uid.ToString()) == null; });
            this.taskList = new TaskList(taskTable);

            //this.sessionTable = new SessionTable("Sessions", db);
            //this.sessionUidGenerator = new UidGenerator(
            //    ApplicationData.Current.RoamingSettings,
            //    "SessionUids",
            //    (uid) => { return this.sessionTable.Get(uid.ToString()) == null; });

            //this.intervalTable = new IntervalTable("Intervals", db);
            //this.intervalUidGenerator = new UidGenerator(
            //    ApplicationData.Current.RoamingSettings,
            //    "IntervalUids",
            //    (uid) => { return this.intervalTable.Get(uid.ToString()) == null; });

            this.session = new Session(1);
        }

        // Destructors
        ~TaskAide()
        {
            this.StopSession();
        }

        // Properties
        public ITask ActiveTask { get; private set; }

        public ISystemTime Time { get => this.systemTime; }

        // Methods
        public ITask AddTask(string taskName)
        {
            var task = new Task(this.taskUidGenerator.NextUid(), taskName);
            this.taskList.Add(task);
            return task;
        }

        public ITask GetTask(int taskId)
        {
            return this.taskList.Get(taskId);
        }

        public ITaskList GetTaskList()
        {
            return this.taskList;
        }

        public void RemoveTask(ITask task)
        {
            // TODO: prevent removal of active task
            this.taskList.Remove(task);
        }

        public void RenameTask(ITask task, string name)
        {
            ((Task)task).Name = name;
        }

        public IDateTimeZone StartSession()
        {
            var dateTime = new DateTimeZone(this.systemTime.UtcNow, this.systemTime.LocalTimeZone);
            this.timer.Start();
            this.session.Start(dateTime);
            return dateTime;
        }

        public IInterval SwitchTasks(ITask task)
        {
            IInterval im = null;
            if ((this.ActiveTask == null) || (task.TID != this.ActiveTask.TID))
            {
                this.ActiveTask = task;
                im = this.StartNewInterval(this.ActiveTask, this.session);
            }
            return im;
        }

        public void StopSession()
        {
            this.timer.Stop();
            this.session.Stop();
            if (this.session.ActiveInterval != null)
            {
                this.session.ActiveInterval.Span = this.timer.SecondsElapsed - this.session.ActiveInterval.Start;
                //this.intervalTable.UpdateSpan(this.session.ActiveInterval);
            }
        }

        public void UserChangedInterval(IInterval interval, TimeSpan startDelta, TimeSpan spanDelta)
        {
            ((Interval)interval).UserChanged(startDelta, spanDelta);
        }

        // Helpers
        private Interval CreateInterval(ITask task, Session session)
        {
            var interval = new Interval(1, task.TID, session.SID);
            //var interval = new Interval(this.intervalUidGenerator.NextUid(), task.UID, session.SID);

            interval.Start = this.timer.SecondsElapsed;
            interval.Span = TimeSpan.Zero;
            interval.Predecessor = session.ActiveInterval;
            //this.intervalTable.Add(interval);

            this.timer.SecondElapsed = () => interval.SecondsIncrement();

            if (session.ActiveInterval != null)
            {
                session.ActiveInterval.Span = interval.Start - session.ActiveInterval.Start;
                session.ActiveInterval.Successor = interval;
                //this.intervalTable.UpdateSpan(session.ActiveInterval);
            }

            session.ActiveInterval = interval;
            return interval;
        }

        private Interval StartNewInterval(ITask task, Session session)
        {
            ((Task)task).Associate(session);
            var interval = this.CreateInterval(task, session);
            this.session.Add(interval);
            return interval;
        }
    }
}
