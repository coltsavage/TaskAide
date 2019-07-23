using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace zCompany.TaskAide
{
    internal class TaskList : ITaskList
    {
        // Fields
        private TaskTable table;

        // Constructors
        public TaskList(TaskTable table)
        {
            this.table = table;
        }

        // Events
        public event EventHandler<TaskListChangedArgs> TaskListChanged;

        // Properties
        public List<ITask> Tasks
        {
            get => this.GetTaskList();
        }

        // Methods
        public virtual bool Add(Task task)
        {
            bool success = this.table.Add(task);
            if (success)
            {
                task.PropertyChanged += this.OnTaskPropertyChanged;
                this.TaskListChanged?.Invoke(this, new TaskListChangedArgs(TaskListChangedArgs.OperationType.Add, task));
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
                this.TaskListChanged?.Invoke(this, new TaskListChangedArgs(TaskListChangedArgs.OperationType.Remove, taskRef));
            }
            return success;
        }

        // Event Handlers
        protected virtual void OnTaskPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Name")
            {
                var task = (Task)sender;
                this.table.UpdateName(task);
                this.TaskListChanged?.Invoke(this, new TaskListChangedArgs(TaskListChangedArgs.OperationType.Rename, (ITask)sender));
            }
        }

        // Helpers
        private List<ITask> GetTaskList()
        {
            List<Task> list = this.table.GetTaskList();
            return list.ConvertAll(new Converter<Task, ITask>(
                (t) =>
                {
                    t.PropertyChanged += this.OnTaskPropertyChanged;
                    return t;
                }));
        }
    }
}