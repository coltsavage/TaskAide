using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace zCompany.TaskAide
{
    internal class TaskList : ITaskList
    {
        // Fields
        private TaskTable table;
        private ObservableCollection<ITask> tasks;

        // Constructors
        public TaskList(TaskTable table)
        {
            this.table = table;
            this.tasks = this.GetListFromStorage();
            this.Tasks = new ReadOnlyObservableCollection<ITask>(this.tasks);
        }

        // Properties
        public ReadOnlyObservableCollection<ITask> Tasks { get; private set; }

        // Methods
        public virtual bool Add(Task task)
        {
            bool success = this.table.Add(task);
            if (success)
            {
                task.PropertyChanged += this.OnTaskPropertyChanged;
                this.tasks.Add(task);
            }
            return success;
        }

        public Task Get(int taskId)
        {
            return this.table.Get(taskId.ToString());
        }

        public virtual bool Remove(ITask taskRef)
        {
            bool success = this.table.Remove(taskRef.TID.ToString());
            if (success)
            {
                taskRef.PropertyChanged -= this.OnTaskPropertyChanged;
                this.tasks.Remove(taskRef);
            }
            return success;
        }

        // Event Handlers
        protected virtual void OnTaskPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Name")
            {
                this.table.UpdateName((Task)sender);
            }
        }

        // Helpers
        private ObservableCollection<ITask> GetListFromStorage()
        {
            var tasks = new ObservableCollection<ITask>();
            foreach (var task in this.table.GetTaskList())
            {
                task.PropertyChanged += this.OnTaskPropertyChanged;
                tasks.Add(task);
            }
            return tasks;
        }
    }
}