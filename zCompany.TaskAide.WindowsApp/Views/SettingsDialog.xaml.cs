﻿using System;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace zCompany.TaskAide.WindowsApp
{
    internal sealed partial class SettingsDialog : ContentDialog, INotifyPropertyChanged
    {
        // Fields
        private AppSettings appSettings;
        
        // Constructors
        public SettingsDialog(ITaskListViewModel taskListViewModel, ITask currentTask, AppSettings appSettings)
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

        public event EventHandler<TaskColorChangedArgs> TaskColorChanged;

        // Properties
        private SolidColorBrush selectedTaskColorBrush;
        public SolidColorBrush SelectedTaskColorBrush
        {
            get { return this.selectedTaskColorBrush; }
            set
            {
                this.selectedTaskColorBrush = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedTaskColorBrush"));
            }
        }

        public ITask SelectedTaskOnOpen { get; }

        public ITaskListViewModel TaskListViewModel { get; }

        // Methods
        public Color GetColorOfSelectedTask()
        {
            if (DialogTaskListView.SelectedItem == null)
            {
                return Colors.White;
            }
            else
            {
                return this.appSettings.GetTaskColor((ITask)DialogTaskListView.SelectedItem);
            }
        }

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

            this.TaskColorChanged?.Invoke(this, new TaskColorChangedArgs(task, color));
            TaskColorFlyout.Hide();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs args)
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
