using System;
using System.Collections.Generic;

namespace zCompany.TaskAide
{
    public interface ITaskList
    {
        // Events
        event EventHandler<TaskListChangedArgs> TaskListChanged;

        // Properties
        List<ITask> Tasks { get; }
    }
}
