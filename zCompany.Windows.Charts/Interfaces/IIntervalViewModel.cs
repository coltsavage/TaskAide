using System;
using System.ComponentModel;

namespace zCompany.Windows.Charts
{
    public interface IIntervalViewModel : INotifyPropertyChanged
    {
        // Properties
        int Span { get; }

        int Start { get; }
    }
}