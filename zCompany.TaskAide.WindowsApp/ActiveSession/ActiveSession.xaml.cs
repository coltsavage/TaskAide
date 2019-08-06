using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace zCompany.TaskAide.WindowsApp
{
    public sealed partial class ActiveSession : Page
    {
        // Constructors
        public ActiveSession()
        {
            this.InitializeComponent();
        }

        // Properties
        internal ITaskList TaskListViewModel { get; private set; }

        // Event Handlers
        private void ActiveSession_OnLoaded(object sender, RoutedEventArgs args)
        {

        }

        private void ActiveSession_OnUnloaded(object sender, RoutedEventArgs args)
        {
            App.Events.ActiveTaskChanged -= this.OnActiveTaskChanged;
        }

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

        private void OnActiveTaskChanged(object sender, ActiveTaskChangedEventArgs args)
        {
            this.TaskListView.SelectedItem = args.ActiveTask;
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ActiveSessionAutomationPeer(this);
        }

        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            App.Events.ActiveTaskChanged += this.OnActiveTaskChanged;

            this.TaskListViewModel = App.State.TaskList;
            this.TaskListView.SelectedItem = App.State.ActiveTask;

            this.Session.Model = App.State.ActiveSession;
        }

        private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (((ComboBox)sender).SelectedItem != null)
            {
                App.Events.Raise(new UserSwitchedTasksEventArgs((ITask)args.AddedItems.First()));
            }
        }
    }
}
