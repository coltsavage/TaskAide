using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace zCompany.TaskAide
{
    internal class Task : ITask
    {
        // Fields
        private List<Session> sessions;

        // Constructors
        private Task()
        {
            this.sessions = new List<Session>();
        }

        public Task(int tid, string name)
            : this()
        {
            this.tid = tid;
            this.name = name;
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        private int actual;
        public int Actual
        {
            get { return this.actual; }
            set
            {
                this.actual = value;
                this.NotifyPropertyChanged();
            }
        }

        private int estimate;
        public int Estimate
        {
            get { return this.estimate; }
            set
            {
                this.estimate = value;
                this.NotifyPropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.NotifyPropertyChanged();
            }
        }

        private Project project;
        public Project Project
        {
            get { return this.project; }
            set
            {
                this.project = value;
                this.NotifyPropertyChanged();
            }
        }

        private Tags tags;
        public Tags Tags
        {
            get { return this.tags; }
            set
            {
                this.tags = value;
                this.NotifyPropertyChanged();
            }
        }

        private int tid;
        public int TID
        {
            get => this.tid;
        }

        // Methods
        public void Associate(Session session)
        {
            bool notPresent = sessions.Contains(session);
            if (notPresent)
            {
                sessions.Add(session);
            }
        }

        public bool Equals(ITask other)
        {
            return this.TID == other?.TID;
        }

        public override bool Equals(object obj)
        {
            return (obj != null && obj is ITask) ? this.Equals((ITask)obj) : false;
        }

        public override int GetHashCode()
        {
            return this.TID.GetHashCode();
        }

        public static bool operator ==(Task task1, Task task2)
        {
            if (((object)task1) == null || ((object)task2) == null)
            {
                return Object.Equals(task1, task2);
            }

            return task1.Equals(task2);
        }

        public static bool operator !=(Task task1, Task task2)
        {
            return !(task1 == task2);
        }

        // Helpers
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
