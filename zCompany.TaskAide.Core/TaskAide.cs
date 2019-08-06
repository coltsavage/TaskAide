using System;
using Windows.Storage;
using System.Collections.Generic;
using zCompany.Utilities;

namespace zCompany.TaskAide
{
    public class TaskAide : ITaskAide
    {
        // Fields
        private int intervalTempUidGenerator;
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
        }

        // Destructors
        ~TaskAide()
        {

        }

        // Properties
        public ISession ActiveSession { get => this.session ?? this.CreateSession(); }

        public ITask ActiveTask { get; private set; }

        public ITaskList TaskList { get => this.taskList; }

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

        public void RemoveTask(ITask task)
        {
            // TODO: prevent removal of active task
            this.taskList.Remove(task);
        }

        public void RenameTask(ITask task, string name)
        {
            ((Task)task).Name = name;
        }

        public void SwitchTasks(ITask task)
        {
            if ((this.ActiveTask == null) || (task.TID != this.ActiveTask.TID))
            {
                this.ActiveTask = task;
                this.StartNewInterval(this.ActiveTask, this.session);
            }
        }

        public void UserChangedInterval(IInterval interval, TimeSpan startDelta, TimeSpan spanDelta)
        {
            ((Interval)interval).UserChanged(startDelta, spanDelta);
        }

        // Event Handlers
        public void OnSystemRewound(object sender, RewoundEventArgs args)
        {
            var amount = args.Amount;
            var activeInterval = (Interval)this.session.ActiveInterval;

            while ((activeInterval != null) && (amount.TotalSeconds > 0))
            {
                if (activeInterval.Span > amount)
                {
                    activeInterval.Span -= amount;
                    break;
                }
                else
                {
                    amount -= activeInterval.Span;

                    if (activeInterval.Predecessor != null)
                    {
                        activeInterval = activeInterval.Predecessor;
                        activeInterval.Successor = null;
                        this.timer.SecondElapsed = () => activeInterval.SecondsIncrement();
                        this.ActiveTask = this.GetTask(activeInterval.TaskId);
                    }
                    else
                    {
                        activeInterval = null;
                        this.timer.SecondElapsed = null;
                        this.ActiveTask = null;
                    }

                    this.session.Pop();
                    this.session.ActiveInterval = activeInterval;
                }
            }
        }

        // Helpers
        private Interval CreateInterval(ITask task, Session session)
        {
            var interval = new Interval(++intervalTempUidGenerator, task.TID, session.SID);
            //var interval = new Interval(this.intervalUidGenerator.NextUid(), task.UID, session.SID);
            var priorInterval = (Interval)session.ActiveInterval;

            interval.Start = this.timer.SecondsElapsed;
            interval.Span = TimeSpan.Zero;
            interval.Predecessor = priorInterval;
            //this.intervalTable.Add(interval);

            this.timer.SecondElapsed = () => interval.SecondsIncrement();

            if (priorInterval != null)
            {
                priorInterval.Span = interval.Start - priorInterval.Start;
                priorInterval.Successor = interval;
                //this.intervalTable.UpdateSpan(priorInterval);
            }

            session.ActiveInterval = interval;
            return interval;
        }

        private Session CreateSession()
        {
            var dateTime = new DateTimeZone(this.systemTime.UtcNow, this.systemTime.LocalTimeZone);
            this.timer.Start();
            this.session = new Session(1, dateTime);
            return this.session;
        }

        private void StartNewInterval(ITask task, Session session)
        {
            ((Task)task).Associate(session);
            var interval = this.CreateInterval(task, session);
            session.Push(interval);
        }
    }
}
