using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using zCompany.Utilities;

namespace zCompany.TaskAide.WindowsApp
{
    internal sealed partial class DevTimeBar : UserControl
    {
        // Constructors
        public DevTimeBar()
        {
            this.InitializeComponent();

            ((SystemTimeDev)App.State.Time).SpeedMultiplier = 128;
        }

        // Properties
        public SystemTimeDev SystemTime { get => (SystemTimeDev)App.State.Time; }

        // Event Handlers
        private void FastForwardButton_Click(object sender, RoutedEventArgs e)
        {
            int mins = Convert.ToInt32(this.JumpAmountTextBox.Text);
            this.SystemTime.FastForward(mins);
            this.JumpAmountTextBox.Text = "0";
        }

        private void JumpAmountTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.JumpAmountTextBox.SelectAll();
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
    }
}
