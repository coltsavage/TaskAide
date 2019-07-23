using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace zCompany.TaskAide.WindowsApp
{
    internal sealed partial class IntervalDialog : ContentDialog
    {
        // Fields
        private IInterval interval;
        
        // Constructors
        public IntervalDialog(IInterval interval, ITask task)
        {
            this.InitializeComponent();

            this.interval = interval;

            this.Span = (int)(interval.Span.TotalSeconds);
            this.Start = interval.Start.ToString("H:mm");

            this.TaskName = task.Name;
        }

        // Events
        //public event EventHandler<UserChangedIntervalArgs> IntervalChanged;

        // Properties
        public int Span { get; set; }

        public string Start { get; set; }

        public string TaskName { get; set; }

        // Event Handlers
        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            int startDelta = (int)Math.Ceiling((TimeSpan.ParseExact(StartValueTextBox.Text, "H:mm", null) - TimeSpan.ParseExact(this.Start, "H:mm", null)).TotalMinutes);
            int spanDelta = Convert.ToInt32(SpanValueTextBox.Text) - this.Span;

            if ((startDelta != 0) || (spanDelta != 0))
            {
                //this.IntervalChanged?.Invoke(this, new UserChangedIntervalArgs(
                //    new TimeSpan(0, startDelta, 0),
                //    new TimeSpan(0, spanDelta, 0)
                //    ));
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs args)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}
