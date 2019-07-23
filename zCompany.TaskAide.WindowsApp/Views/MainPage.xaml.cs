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
    internal sealed partial class MainPage : Page
    {
        // Fields
        private TaskAide taskAide;
        private AppSettings appSettings;

        // Constructors
        public MainPage()
        {
            this.InitializeComponent();

            Database db = new Database("Filename=TaskTracker.db");

            var systemTime = new SystemTimeDev(DateTimeOffset.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            systemTime.SpeedMultiplier = 64;

            this.TimeBar.SystemTime = systemTime;

            this.taskAide = new TaskAide(db, systemTime, new TimerDev(systemTime));
            this.appSettings = new AppSettings();

            this.TaskListViewModel = new TaskListViewModel(this.taskAide.GetTaskList());

            IDateTimeZone dateTime = this.taskAide.StartSession();
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
                this.taskAide.AddTask(textBox.Text);
                AddTaskFlyout.Hide();
            }
        }

        private void Chart_IntervalResized(object sender, IntervalResizedArgs args)
        {
            this.taskAide.UserChangedInterval(
                ((IntervalViewModel)args.ViewModel).Model,
                new TimeSpan(0, args.StartDelta, 0),
                new TimeSpan(0, args.SpanDelta, 0));
        }

        private async void ConfigureButton_Click(object sender, RoutedEventArgs args)
        {
            SettingsDialog config = new SettingsDialog(this.TaskListViewModel, this.taskAide.ActiveTask, this.appSettings);
            config.TaskNameChanged =
                (task, newName) =>
                {
                    this.taskAide.RenameTask(task, newName);

                    var series = (SeriesViewModel)this.Chart.GetSeries(task.TID);
                    if (series != null)
                    {
                        series.Name = newName;
                    }
                };
            config.TaskRemoved =
                (task) =>
                {
                    this.taskAide.RemoveTask(task);
                    this.Chart.RemoveSeries(task.TID);
                };
            config.TaskColorChanged +=
                (object o, TaskColorChangedArgs e) =>
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
                IInterval interval = this.taskAide.StartWorking(task);
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
                Color color = this.appSettings.GetTaskColor(task);
                this.Chart.AddSeries(task.TID, new SeriesViewModel(task.Name, color));
                this.Chart.AddInterval(interval.TaskId, intervalViewModel);
            }
        }
    }
}
