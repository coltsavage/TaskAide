using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace zCompany.Windows.Charts
{
    internal class ChartState : IChartState
    {
        // Constructors
        public ChartState()
        {
            this.FieldOffsetFromAxisOriginInTicks = 0;
            this.PixelsPerTick = 1;
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        private int originOffset;
        public int FieldOffsetFromAxisOriginInTicks
        {
            get { return this.originOffset; }
            set
            {
                this.originOffset = value;
                this.RaisePropertyChanged();
            }
        }
        
        private int pixelsPerTick;
        public int PixelsPerTick
        {
            get { return this.pixelsPerTick; }
            set
            {
                this.pixelsPerTick = value;
                this.RaisePropertyChanged();
            }
        }

        // Helpers
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
