using System;
using System.ComponentModel;
using Windows.UI;

namespace zCompany.Windows.Charts
{
    public interface ISeriesViewModel : INotifyPropertyChanged
    {
        // Properties
        Color Color { get; }

        string Name { get; }
    }
}
