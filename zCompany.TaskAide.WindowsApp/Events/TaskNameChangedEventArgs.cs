using System;

namespace zCompany.TaskAide.WindowsApp
{
    internal class TaskNameChangedEventArgs
    {
        // Constructors
        public TaskNameChangedEventArgs(ITask task, string newName)
        {
            this.Task = task;
            this.NewName = newName;
        }

        // Properties
        public string NewName { get; }

        public ITask Task { get; }
    }
}
