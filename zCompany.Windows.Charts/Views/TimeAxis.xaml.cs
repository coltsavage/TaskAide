using System;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;

namespace zCompany.Windows.Charts
{
    internal sealed partial class TimeAxis : UserControl
    {
        // Constructors
        public TimeAxis()
        {
            this.InitializeComponent();
        }

        // Methods
        public void ConfigureLabels(IAxisLabelProvider labelProvider, int pixelsPerTick)               
        {
            this.container.Children.Clear();
            this.container.Children.Add(this.AxisLine);
            
            int axisLengthInPixels = Convert.ToInt32(this.Resources["Width"]);
            int labelIntervalInPixels = pixelsPerTick * labelProvider.LabelFrequencyInTicks;

            for (int i = 0; i <= axisLengthInPixels; i += labelIntervalInPixels)
            {
                MajorTick tick = new MajorTick();
                tick.X = i;
                tick.Label = labelProvider.LabelText;
                tick.SetValue(Canvas.LeftProperty, i - tick.ActualWidth);

                this.container.Children.Add(tick);
                labelProvider.NextLabel();
            }
        }

        // Event Handlers
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new TimeAxisAutomationPeer(this);
        }
    }
}
