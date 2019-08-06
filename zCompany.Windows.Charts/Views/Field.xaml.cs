using System;
using System.Linq;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace zCompany.Windows.Charts
{
    internal sealed partial class Field : UserControl
    {
        // Fields
        private int intervalCounter;
        private Interval selectedInterval;

        // Constructors
        public Field()
        {
            this.InitializeComponent();

            this.intervalCounter = 0;
        }

        // Methods
        public int Add(Interval interval)
        {
            interval.InstanceNumber = this.intervalCounter++;
            interval.Height = container.Height;
            interval.PointerPressed += this.OnPointerPressedInInterval;
            container.Children.Add(interval);
            return interval.InstanceNumber;
        }

        public bool Remove(int intervalNumber)
        {
            var interval = this.container.Children.FirstOrDefault((i) => ((Interval)i).InstanceNumber == intervalNumber);
            if (interval != null)
            {
                container.Children.Remove(interval);
                interval.PointerPressed -= this.OnPointerPressedInInterval;
            }
            return (interval != null);
        }

        // Event Handlers
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FieldAutomationPeer(this);
        }

        private void OnPointerPressedInInterval(object sender, PointerRoutedEventArgs args)
        {
            args.Handled = true;
            var intervalPressed = (Interval)sender;

            if (this.selectedInterval == null)
            {
                intervalPressed.IsSelected = true;
                this.selectedInterval = intervalPressed;
            }
            else
            {
                if (this.selectedInterval == intervalPressed)
                {
                    intervalPressed.IsSelected = false;
                    this.selectedInterval = null;
                }
                else
                {
                    this.selectedInterval.IsSelected = false;
                    intervalPressed.IsSelected = true;
                    this.selectedInterval = intervalPressed;
                }
            }
        }
    }
}
