using System;
using System.ComponentModel;

namespace zCompany.TaskAide
{
    public interface IInterval : INotifyPropertyChanged
    {
        // Properties
        int IID { get; }

        TimeSpan Span { get; }

        TimeSpan Start { get; }

        int TaskId { get; }
    }
}
