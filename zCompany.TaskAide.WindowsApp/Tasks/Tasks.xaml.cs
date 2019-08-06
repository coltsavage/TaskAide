﻿using System;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace zCompany.TaskAide.WindowsApp
{
    public sealed partial class Tasks : Page, INotifyPropertyChanged
    {
        // Constructors
        public Tasks()
        {
            this.InitializeComponent();
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        internal Color SelectedTaskColor
        {
            get { return this.selectedTaskColorBrush.Color; }
        }

        private SolidColorBrush selectedTaskColorBrush;
        internal SolidColorBrush SelectedTaskColorBrush
        {
            get { return this.selectedTaskColorBrush; }
            set
            {
                this.selectedTaskColorBrush = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedTaskColorBrush"));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedTaskColor"));
            }
        }

        internal ITask SelectedTaskOnOpen { get; private set; }

        internal ITaskListViewModel TaskListViewModel { get; private set; }

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
            App.Settings.SetTaskColor(task, color);

            App.Events.Raise(new TaskColorChangedEventArgs(task, color));
            TaskColorFlyout.Hide();
        }

        private void DeleteRequestor_Click(object sender, RoutedEventArgs args)
        {
            var task = (ITask)DialogTaskListView.SelectedItem;
            App.Settings.RemoveTask(task);
            App.Events.Raise(new TaskRemovedEventArgs(task));
            DialogTaskListView.SelectedIndex = 0;
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void DialogTaskListView_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (DialogTaskListView.SelectedValue != null)
            {
                Color color = App.Settings.GetTaskColor((ITask)DialogTaskListView.SelectedItem);
                this.SelectedTaskColorBrush = new SolidColorBrush(color);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            this.TaskListViewModel = new TaskListViewModel(App.State.TaskList);
            this.SelectedTaskOnOpen = App.State.ActiveTask;

            if (this.SelectedTaskOnOpen == null)
            {
                this.SelectedTaskColorBrush = new SolidColorBrush(Colors.White);
            }
            else
            {
                this.SelectedTaskColorBrush = new SolidColorBrush(App.Settings.GetTaskColor(this.SelectedTaskOnOpen));
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
                App.Events.Raise(new TaskNameChangedEventArgs((ITask)DialogTaskListView.SelectedItem, ((TextBox)sender).Text));
                RenameTaskFlyout.Hide();
            }
        }
    }
}
