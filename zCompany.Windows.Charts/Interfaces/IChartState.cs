using System.ComponentModel;

namespace zCompany.Windows.Charts
{
    internal interface IChartState : INotifyPropertyChanged
    {
        // Properties
        int FieldOffsetFromAxisOriginInTicks { get; }

        int PixelsPerTick { get; }
    }
}