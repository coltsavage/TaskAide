using System;

namespace zCompany.TaskAide.WindowsApp
{
    internal class TaskAddedEventArgs
    {
        // Constructors
        public TaskAddedEventArgs(string name)
        {
            this.Name = name;
        }

        // Properties
        public string Name { get; }
    }
}
