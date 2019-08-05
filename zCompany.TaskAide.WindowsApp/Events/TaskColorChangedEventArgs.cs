using Windows.UI;

namespace zCompany.TaskAide.WindowsApp
{
    internal class TaskColorChangedEventArgs
    {
        // Constructors
        public TaskColorChangedEventArgs(ITask task, Color color)
        {
            this.Task = task;
            this.Color = color;
        }

        // Properties
        public Color Color { get; }

        public ITask Task { get; }
    }
}
