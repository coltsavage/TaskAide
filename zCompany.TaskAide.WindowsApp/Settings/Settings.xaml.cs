using System;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;

namespace zCompany.TaskAide.WindowsApp
{
    public sealed partial class Settings : Page
    {
        // Constructors
        public Settings()
        {
            this.InitializeComponent();
        }

        // Event Handlers
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new SettingsAutomationPeer(this);
        }
    }
}
