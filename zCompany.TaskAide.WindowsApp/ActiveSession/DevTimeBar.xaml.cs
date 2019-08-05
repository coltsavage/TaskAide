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
        }

        // Properties
        public SystemTimeDev SystemTime { get; set; }

        // Event Handlers
        private void JumpAheadButton_Click(object sender, RoutedEventArgs e)
        {
            int mins = Convert.ToInt32(this.JumpAheadTextBox.Text);
            this.SystemTime.Progress(mins);
            this.JumpAheadTextBox.Text = "0";
        }

        private void JumpAheadTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.JumpAheadTextBox.SelectAll();
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
