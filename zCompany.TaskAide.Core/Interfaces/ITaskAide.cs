using zCompany.Utilities;

namespace zCompany.TaskAide
{
    public interface ITaskAide
    {
        // Properties
        ISession ActiveSession { get; }

        ITask ActiveTask { get; }

        ITaskList TaskList { get; }

        ISystemTime Time { get; }

        // Methods
        ITask GetTask(int taskId);
    }
}