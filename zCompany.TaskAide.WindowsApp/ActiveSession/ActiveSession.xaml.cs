﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using zCompany.Utilities;
using zCompany.Windows.Charts;

namespace zCompany.TaskAide.WindowsApp
{
    internal sealed partial class ActiveSession : Page
    {
        // Fields
        private Dictionary<int, int> intervalLookup;

        // Constructors
        public ActiveSession()
        {
            this.InitializeComponent();

            this.intervalLookup = new Dictionary<int, int>();
            this.TaskListViewModel = new TaskListViewModel(App.State.TaskList);

            ((INotifyCollectionChanged)App.State.ActiveSession.Intervals).CollectionChanged += OnIntervalCollectionChanged;

            IDateTimeZone dateTime = App.State.ActiveSession.DateTimeStart;
            ((SystemTimeDev)App.State.Time).ActiveSessionStartTime = dateTime.UtcTicks;
            this.Chart.ConfigureAxisLabels(new TimeLabelProvider(dateTime, 30));
        }

        // Properties
        public ITaskListViewModel TaskListViewModel { get; set; }

        // Event Handlers
        private void AddTaskFlyout_Closed(object sender, object args)
        {
            AddTaskFlyoutTextBox.Text = string.Empty;
        }

        private void AddTaskFlyoutTextBox_KeyDown(object sender, KeyRoutedEventArgs args)
        {
            if (args.Key == global::Windows.System.VirtualKey.Enter)
            {
                var textBox = (TextBox)sender;
                App.Events.Raise(new TaskAddedEventArgs(textBox.Text));
                AddTaskFlyout.Hide();
            }
        }

        private void Chart_IntervalResized(object sender, IntervalResizedEventArgs args)
        {
            App.Events.Raise(new UserChangedIntervalEventArgs(
                ((IntervalViewModel)args.ViewModel).Model,
                new TimeSpan(0, args.StartDelta, 0),
                new TimeSpan(0, args.SpanDelta, 0)
                ));
        }

        private async void ConfigureButton_Click(object sender, RoutedEventArgs args)
        {
            Tasks config = new Tasks(this.TaskListViewModel, App.State.ActiveTask, App.Settings);

            config.TaskNameChanged =
                (task, newName) =>
                {
                    App.Events.Raise(new TaskNameChangedEventArgs(task, newName));

                    var series = (SeriesViewModel)this.Chart.GetSeries(task.TID);
                    if (series != null)
                    {
                        series.Name = newName;
                    }
                };

            config.TaskRemoved =
                (task) =>
                {
                    App.Events.Raise(new TaskRemovedEventArgs(task));
                    this.Chart.RemoveSeries(task.TID);
                };

            config.TaskColorChanged +=
                (object o, TaskColorChangedEventArgs e) =>
                {
                    var series = (SeriesViewModel)this.Chart.GetSeries(e.Task.TID);
                    if (series != null)
                    {
                        series.Color = e.Color;
                    }
                    else
                    {
                        this.Chart.AddSeries(e.Task.TID, new SeriesViewModel(e.Task.Name, e.Color));
                    }
                };

            await config.ShowAsync();
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

        private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (((ComboBox)sender).SelectedItem != null)
            {
                App.Events.Raise(new UserSwitchedTasksEventArgs((ITask)args.AddedItems.First()));
            }
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
            this.TaskListView.SelectedItem = App.State.ActiveTask;
        }
    }
}
