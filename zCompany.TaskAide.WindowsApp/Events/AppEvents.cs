using System;

namespace zCompany.TaskAide.WindowsApp
{
    internal class AppEvents
    {
        // Constructors
        internal AppEvents()
        {

        }

        // Events
        internal event EventHandler<ActiveTaskChangedEventArgs> ActiveTaskChanged;

        internal event EventHandler<TaskAddedEventArgs> TaskAdded;

        internal event EventHandler<TaskColorChangedEventArgs> TaskColorChanged;

        internal event EventHandler<TaskNameChangedEventArgs> TaskNameChanged;

        internal event EventHandler<TaskRemovedEventArgs> TaskRemoved;

        internal event EventHandler<UserChangedIntervalEventArgs> UserChangedInterval;

        internal event EventHandler<UserSwitchedTasksEventArgs> UserSwitchedTasks;

        // Methods
        internal void Raise(object eventArgs)
        {
            switch (eventArgs)
            {
                case ActiveTaskChangedEventArgs args:
                    this.ActiveTaskChanged?.Invoke(this, args);
                    break;
                case TaskAddedEventArgs args:
                    this.TaskAdded?.Invoke(this, args);
                    break;
                case TaskColorChangedEventArgs args:
                    this.TaskColorChanged?.Invoke(this, args);
                    break;
                case TaskNameChangedEventArgs args:
                    this.TaskNameChanged?.Invoke(this, args);
                    break;
                case TaskRemovedEventArgs args:
                    this.TaskRemoved?.Invoke(this, args);
                    break;
                case UserChangedIntervalEventArgs args:
                    this.UserChangedInterval?.Invoke(this, args);
                    break;
                case UserSwitchedTasksEventArgs args:
                    this.UserSwitchedTasks?.Invoke(this, args);
                    break;
                default:
                    break;
            }
        }
    }
}
