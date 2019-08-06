using System;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;

namespace zCompany.Windows.Charts
{
    internal sealed partial class MajorTick : UserControl
    {

        // Constructors
        public MajorTick()
        {
            this.InitializeComponent();
        }

        // Properties
        public int InstanceNumber { get; set; }

        private string label;
        public string Label
        {
            get => this.label;
            set
            {
                this.label = value;
                this.Bindings.Update();
            }
        }

        private int x;
        public int X
        {
            get => this.x;
            set
            {
                this.x = value;
                this.Bindings.Update();
            }
        }

        // Event Handlers
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new MajorTickAutomationPeer(this);
        }
    }
}
