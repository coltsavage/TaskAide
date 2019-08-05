using System;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace zCompany.Windows.Charts
{
    internal sealed partial class Field : UserControl
    {
        // Fields
        private Interval selectedInterval;

        // Constructors
        public Field()
        {
            this.InitializeComponent();
        }

        // Methods
        public void Add(Interval view)
        {
            view.Height = container.Height;
            view.PointerPressed += this.OnPointerPressedInInterval;
            container.Children.Add(view);
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
