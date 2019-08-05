using System;

namespace zCompany.TaskAide.WindowsApp
{
    internal class UserSwitchedTasksEventArgs
    {
        // Constructors
        public UserSwitchedTasksEventArgs(ITask task)
        {
            this.Task = task;
        }

        // Properties
        public ITask Task { get; }
    }
}
