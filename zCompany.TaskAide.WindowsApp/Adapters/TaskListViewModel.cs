using System.Collections.ObjectModel;

namespace zCompany.TaskAide.WindowsApp
{
    internal class TaskListViewModel : ITaskListViewModel
    {
        // Fields
        private ITaskList model;

        // Constructors
        public TaskListViewModel(ITaskList model)
        {
            this.model = model;
            this.model.TaskListChanged += this.OnModelChanged;

            this.Tasks = new ObservableCollection<ITask>(model.Tasks);
        }

        // Destructors
        ~TaskListViewModel()
        {
            this.model.TaskListChanged -= this.OnModelChanged;
        }

        // Properties
        public ObservableCollection<ITask> Tasks { get; set; }

        // Event Handlers
        private void OnModelChanged(object sender, TaskListChangedArgs args)
        {
            switch (args.Operation)
            {
                case TaskListChangedArgs.OperationType.Add:
                    this.Tasks.Add(args.Task);
                    break;
                case TaskListChangedArgs.OperationType.Remove:
                    this.Tasks.Remove(args.Task);
                    break;
                case TaskListChangedArgs.OperationType.Rename:
                    //handled innately from task name-changed propogation events
                    break;
                default:
                    // undocumented change
                    break;
            }
        }
    }
}
