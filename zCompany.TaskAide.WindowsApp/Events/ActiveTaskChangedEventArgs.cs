using System;

namespace zCompany.TaskAide.WindowsApp
{
    internal class ActiveTaskChangedEventArgs
    {
        // Constructors
        public ActiveTaskChangedEventArgs(ITask activeTask)
        {
            this.ActiveTask = activeTask;
        }

        // Properties
        public ITask ActiveTask { get; }
    }
}
