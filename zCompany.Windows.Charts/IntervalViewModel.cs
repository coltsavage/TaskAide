using System;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace zCompany.Windows.Charts
{
    internal class IntervalViewModel : IIntervalViewModel
    {
        // Fields
        private IChartState chartState;
        private IIntervalViewModel intervalViewModel;
        private ISeriesViewModel seriesViewModel;

        // Constructors
        public IntervalViewModel(IIntervalViewModel intervalViewModel, ISeriesViewModel seriesViewModel, IChartState chartState)
        {
            this.intervalViewModel = intervalViewModel;
            this.intervalViewModel.PropertyChanged += this.OnViewModelPropertyChanged;

            this.seriesViewModel = seriesViewModel;
            this.seriesViewModel.PropertyChanged += this.OnViewModelPropertyChanged;

            this.chartState = chartState;
            this.chartState.PropertyChanged += this.OnChartStatePropertyChanged;
        }

        // Destructors
        ~IntervalViewModel()
        {
            this.intervalViewModel.PropertyChanged -= this.OnViewModelPropertyChanged;
            this.seriesViewModel.PropertyChanged -= this.OnViewModelPropertyChanged;
            this.chartState.PropertyChanged -= this.OnChartStatePropertyChanged;
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public SolidColorBrush Color { get => new SolidColorBrush(this.seriesViewModel.Color); }

        public string Name { get => this.seriesViewModel.Name; }

        public IIntervalViewModel OriginalModel { get => this.intervalViewModel; }

        public int Span { get => this.intervalViewModel.Span * this.chartState.PixelsPerTick; }

        public int Start { get => (this.intervalViewModel.Start + this.chartState.FieldOffsetFromAxisOriginInTicks) * this.chartState.PixelsPerTick; }

        // Event Handlers
        protected virtual void OnChartStatePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "FieldOffsetFromAxisOriginInTicks")
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Start"));
            }
            else if(args.PropertyName == "PixelsPerTick")
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Span"));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Start"));
            }             
        }

        protected virtual void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(args.PropertyName));
        }
    }
}
