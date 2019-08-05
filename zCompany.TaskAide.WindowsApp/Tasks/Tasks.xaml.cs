using System;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace zCompany.TaskAide.WindowsApp
{
    internal sealed partial class Tasks : ContentDialog, INotifyPropertyChanged
    {
        // Fields
        private AppSettings appSettings;
        
        // Constructors
        public Tasks(ITaskListViewModel taskListViewModel, ITask currentTask, AppSettings appSettings)
        {
            this.InitializeComponent();

            this.appSettings = appSettings;

            this.TaskListViewModel = taskListViewModel;
            this.SelectedTaskOnOpen = currentTask;

            if (this.SelectedTaskOnOpen == null)
            {
                this.SelectedTaskColorBrush = new SolidColorBrush(Colors.White);
            }
            else
            {
                this.SelectedTaskColorBrush = new SolidColorBrush(this.appSettings.GetTaskColor(this.SelectedTaskOnOpen));
            }
        }

        // Delegates
        public Action<ITask, string> TaskNameChanged;

        public Action<ITask> TaskRemoved;

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<TaskColorChangedEventArgs> TaskColorChanged;

        // Properties
        public Color SelectedTaskColor
        {
            get { return this.selectedTaskColorBrush.Color; }
        }

        private SolidColorBrush selectedTaskColorBrush;
        public SolidColorBrush SelectedTaskColorBrush
        {
            get { return this.selectedTaskColorBrush; }
            set
            {
                this.selectedTaskColorBrush = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedTaskColorBrush"));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedTaskColor"));
            }
        }

        public ITask SelectedTaskOnOpen { get; }

        public ITaskListViewModel TaskListViewModel { get; }

        // Event Handlers
        private void ColorCancel_Click(object sender, RoutedEventArgs args)
        {
            TaskColorFlyout.Hide();
        }

        private void ColorCommit_Click(object sender, RoutedEventArgs args)
        {
            var task = (ITask)DialogTaskListView.SelectedItem;
            Color color = TaskColorPicker.Color;

            this.SelectedTaskColorBrush = new SolidColorBrush(color);
            this.appSettings.SetTaskColor(task, color);

            this.TaskColorChanged?.Invoke(this, new TaskColorChangedEventArgs(task, color));
            TaskColorFlyout.Hide();
        }

        private void DeleteRequestor_Click(object sender, RoutedEventArgs args)
        {
            var task = (ITask)DialogTaskListView.SelectedItem;
            this.appSettings.RemoveTask(task);
            this.TaskRemoved?.Invoke(task);
            DialogTaskListView.SelectedIndex = 0;
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void DialogTaskListView_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (DialogTaskListView.SelectedValue != null)
            {
                Color color = this.appSettings.GetTaskColor((ITask)DialogTaskListView.SelectedItem);
                this.SelectedTaskColorBrush = new SolidColorBrush(color);
            }
        }

        private void RenameTaskFlyout_Closed(object sender, object e)
        {
            RenameTaskFlyoutTextBox.Text = string.Empty;
        }

        private void RenameTaskFlyoutTextBox_KeyDown(object sender, KeyRoutedEventArgs args)
        {
            if (args.Key == global::Windows.System.VirtualKey.Enter)
            {
                this.TaskNameChanged?.Invoke((ITask)DialogTaskListView.SelectedItem, ((TextBox)sender).Text);
                RenameTaskFlyout.Hide();
            }
        }
    }
}
