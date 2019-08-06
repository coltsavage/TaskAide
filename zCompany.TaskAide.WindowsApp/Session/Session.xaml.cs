using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using zCompany.Utilities;
using zCompany.Windows.Charts;

namespace zCompany.TaskAide.WindowsApp
{
    public sealed partial class Session : UserControl
    {
        // Fields
        private Dictionary<int, int> intervalLookup;

        // Constructors
        public Session()
        {
            this.InitializeComponent();

            this.intervalLookup = new Dictionary<int, int>();
        }

        // Properties
        internal ISession Model { get; set; }

        // Event Handlers
        private void Chart_IntervalResized(object sender, IntervalResizedEventArgs args)
        {
            App.Events.Raise(
                new UserChangedIntervalEventArgs(
                    ((IntervalViewModel)args.ViewModel).Model,
                    new TimeSpan(0, args.StartDelta, 0),
                    new TimeSpan(0, args.SpanDelta, 0)));
        }

        private void OnIntervalCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                this.AddIntervalToChart((IInterval)args.NewItems[0]);
            }
            if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                this.RemoveIntervalFromChart((IInterval)args.OldItems[0]);
            }
        }

        private void Session_OnLoaded(object sender, RoutedEventArgs args)
        {
            ((INotifyCollectionChanged)this.Model.Intervals).CollectionChanged += OnIntervalCollectionChanged;
            foreach (var i in this.Model.Intervals)
            {
                this.AddIntervalToChart(i);
            }

            var dateTime = this.Model.DateTimeStart;
            ((SystemTimeDev)App.State.Time).ActiveSessionStartTime = dateTime.UtcTicks;
            this.Chart.ConfigureAxisLabels(new TimeLabelProvider(dateTime, 30));
        }

        private void Session_OnUnloaded(object sender, RoutedEventArgs args)
        {
            ((INotifyCollectionChanged)this.Model.Intervals).CollectionChanged -= OnIntervalCollectionChanged;
        }

        // Helpers
        private void AddIntervalToChart(IInterval interval)
        {
            var intervalViewModel = new IntervalViewModel(interval);
            var intervalNumber = this.Chart.AddInterval(interval.TaskId, intervalViewModel);
            if (intervalNumber < 0)
            {
                var task = App.State.GetTask(interval.TaskId);
                Color color = App.Settings.GetTaskColor(task);
                this.Chart.AddSeries(task.TID, new SeriesViewModel(task.Name, color));
                intervalNumber = this.Chart.AddInterval(interval.TaskId, intervalViewModel);
            }
            this.intervalLookup[interval.IID] = intervalNumber;
        }

        private void RemoveIntervalFromChart(IInterval interval)
        {
            this.Chart.RemoveInterval(this.intervalLookup[interval.IID]);
            App.Events.Raise(new ActiveTaskChangedEventArgs(App.State.ActiveTask));
        }
    }
}
