using System;
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
        // Constructors
        public ActiveSession()
        {
            this.InitializeComponent();
            this.TaskListViewModel = new TaskListViewModel(App.State.GetTaskList());

            IDateTimeZone dateTime = App.State.StartSession();
            this.Chart.ConfigureAxisLabels(new TimeLabelProvider(dateTime, 30));
        }

        // Properties
        public ITaskListViewModel TaskListViewModel { get; set; }

        // Event Handlers
        private void AddTaskFlyout_Closed(object sender, object e)
        {
            AddTaskFlyoutTextBox.Text = string.Empty;
        }

        private void AddTaskFlyoutTextBox_KeyDown(object sender, KeyRoutedEventArgs args)
        {
            if (args.Key == global::Windows.System.VirtualKey.Enter)
            {
                var textBox = (TextBox)sender;
                App.State.AddTask(textBox.Text);
                AddTaskFlyout.Hide();
            }
        }

        private void Chart_IntervalResized(object sender, IntervalResizedEventArgs args)
        {
            App.State.UserChangedInterval(
                ((IntervalViewModel)args.ViewModel).Model,
                new TimeSpan(0, args.StartDelta, 0),
                new TimeSpan(0, args.SpanDelta, 0));
        }

        private async void ConfigureButton_Click(object sender, RoutedEventArgs args)
        {
            Tasks config = new Tasks(this.TaskListViewModel, App.State.ActiveTask, App.Settings);

            config.TaskNameChanged =
                (task, newName) =>
                {
                    App.State.RenameTask(task, newName);

                    var series = (SeriesViewModel)this.Chart.GetSeries(task.TID);
                    if (series != null)
                    {
                        series.Name = newName;
                    }
                };

            config.TaskRemoved =
                (task) =>
                {
                    App.State.RemoveTask(task);
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

        private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (((ComboBox)sender).SelectedItem != null)
            {
                var task = (ITask)args.AddedItems.First();
                IInterval interval = App.State.SwitchTasks(task);
                if (interval != null)
                {
                    this.AddIntervalToChart(interval, task);
                }
            }
        }

        // Helpers
        private void AddIntervalToChart(IInterval interval, ITask task)
        {
            var intervalViewModel = new IntervalViewModel(interval);
            bool success = this.Chart.AddInterval(interval.TaskId, intervalViewModel);
            if (!success)
            {
                Color color = App.Settings.GetTaskColor(task);
                this.Chart.AddSeries(task.TID, new SeriesViewModel(task.Name, color));
                this.Chart.AddInterval(interval.TaskId, intervalViewModel);
            }
        }
    }
}
