using System;

namespace zCompany.TaskAide.WindowsApp
{
    internal class TaskRemovedEventArgs
    {
        // Constructors
        public TaskRemovedEventArgs(ITask task)
        {
            this.Task = task;
        }

        // Properties
        public ITask Task { get; }
    }
}
