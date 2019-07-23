using System;
using System.ComponentModel;

namespace zCompany.TaskAide
{
    public interface ITask : INotifyPropertyChanged, IEquatable<ITask>
    {
        // Properties
        string Name { get; }

        int TID { get; }
    }
}
