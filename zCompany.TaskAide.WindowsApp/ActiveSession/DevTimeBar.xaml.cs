using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using zCompany.Utilities;

namespace zCompany.TaskAide.WindowsApp
{
    public sealed partial class DevTimeBar : UserControl
    {
        // Constructors
        public DevTimeBar()
        {
            this.InitializeComponent();

            ((SystemTimeDev)App.State.Time).SpeedMultiplier = 128;
        }

        // Properties
        internal SystemTimeDev SystemTime { get => (SystemTimeDev)App.State.Time; }

        // Event Handlers
        private void FastForwardButton_Click(object sender, RoutedEventArgs e)
        {
            int mins = Convert.ToInt32(this.JumpAmountTextBox.Text);
            this.SystemTime.FastForward(TimeSpan.FromMinutes(mins));
            this.JumpAmountTextBox.Text = "0";
        }

        private void JumpAmountTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.JumpAmountTextBox.SelectAll();
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new DevTimeBarAutomationPeer(this);
        }

        private void PauseResumeTimeToggle_Click(object sender, RoutedEventArgs e)
        {
            if ((string)this.PauseResumeTimeToggle.Content == "Pause")
            {
                this.SystemTime.Pause();
                this.PauseResumeTimeToggle.Content = "Resume";
            }
            else
            {
                this.SystemTime.Resume();
                this.PauseResumeTimeToggle.Content = "Pause";
            }
        }

        private void RaiseRateButton_Click(object sender, RoutedEventArgs e)
        {
            this.SystemTime.SpeedUp();
        }

        private void ReduceRateButton_Click(object sender, RoutedEventArgs e)
        {
            this.SystemTime.SlowDown();
        }

        private void RewindButton_Click(object sender, RoutedEventArgs e)
        {
            int mins = Convert.ToInt32(this.JumpAmountTextBox.Text);
            this.SystemTime.Rewind(TimeSpan.FromMinutes(mins));
            this.JumpAmountTextBox.Text = "0";
        }

        private void RemoveLastIntervalButton_Click(object sender, RoutedEventArgs args)
        {
            var activeInterval = App.State.ActiveSession.ActiveInterval;
            if (activeInterval != null)
            {
                this.SystemTime.Rewind(activeInterval.Span);
            }            
        }
    }
}
