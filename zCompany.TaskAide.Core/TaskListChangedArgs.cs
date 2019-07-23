using System;

namespace zCompany.TaskAide
{
    public class TaskListChangedArgs : EventArgs
    {
        // Constructors
        public TaskListChangedArgs(OperationType op, ITask task)
        {
            this.Operation = op;
            this.Task = task;
        }

        // Enums
        public enum OperationType { Add, Remove, Rename };

        // Properties
        public OperationType Operation { get; }

        public ITask Task { get; }
    }
}
