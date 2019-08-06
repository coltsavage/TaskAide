using System.Collections.ObjectModel;

namespace zCompany.TaskAide
{
    public interface ITaskList
    {
        // Properties
        ReadOnlyObservableCollection<ITask> Tasks { get; }
    }
}
