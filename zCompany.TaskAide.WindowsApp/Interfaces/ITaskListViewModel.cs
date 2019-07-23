using System;
using System.Collections.ObjectModel;

namespace zCompany.TaskAide.WindowsApp
{
    internal interface ITaskListViewModel
    {
        // Properties
        ObservableCollection<ITask> Tasks { get; }
    }
}