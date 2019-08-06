using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;

namespace zCompany.Windows.Charts
{
    public sealed partial class IntervalChart : UserControl
    {
        // Fields
        private ChartState chartState;
        private Dictionary<int, ISeriesViewModel> seriesList;
        
        // Constructors
        public IntervalChart()
        {
            this.InitializeComponent();

            this.chartState = new ChartState();
            this.chartState.PixelsPerTick = 2;

            this.seriesList = new Dictionary<int, ISeriesViewModel>();
        }

        // Events
        public event EventHandler<IntervalResizedEventArgs> IntervalResized;

        // Methods
        public int AddInterval(int seriesUID, IIntervalViewModel ivm)
        {
            int intervalNumber = -1;
            ISeriesViewModel series;
            bool found = this.seriesList.TryGetValue(seriesUID, out series);
            if (found)
            {
                var interval = new Interval(new IntervalViewModel(ivm, series, this.chartState), this.chartState);
                interval.IntervalResized += this.OnIntervalResized;
                intervalNumber = Field.Add(interval);
            }
            return intervalNumber;
        }

        public void AddSeries(int seriesUID, ISeriesViewModel series)
        {
            this.seriesList.Add(seriesUID, series);
        }

        public void ConfigureAxisLabels(IAxisLabelProvider lableMaker)
        {
            this.chartState.FieldOffsetFromAxisOriginInTicks = -lableMaker.FirstLabelOffsetFromStartInTicks;
            this.Axis.ConfigureLabels(lableMaker, this.chartState.PixelsPerTick);
        }

        public ISeriesViewModel GetSeries(int seriesUID)
        {
            ISeriesViewModel series;
            this.seriesList.TryGetValue(seriesUID, out series);
            return series;
        }

        public bool RemoveInterval(int intervalNumber)
        {
            return this.Field.Remove(intervalNumber);
        }

        public void RemoveSeries(int seriesUID)
        {
            this.seriesList.Remove(seriesUID);
        }

        // Event Handlers
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new IntervalChartAutomationPeer(this);
        }

        private void OnIntervalResized(object sender, IntervalResizedEventArgs args)
        {
            this.IntervalResized?.Invoke(this, args);
        }
    }
}
