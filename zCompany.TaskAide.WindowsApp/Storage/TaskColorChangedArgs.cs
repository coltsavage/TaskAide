using Windows.UI;

namespace zCompany.TaskAide.WindowsApp
{
    internal class TaskColorChangedArgs
    {
        // Constructors
        public TaskColorChangedArgs(ITask task, Color color)
        {
            this.Task = task;
            this.Color = color;
        }

        // Properties
        public Color Color { get; }

        public ITask Task { get; }
    }
}
