using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace zCompany.TaskAide.WindowsApp
{
    internal class SettingsAutomationPeer : FrameworkElementAutomationPeer
    {
        // Constructors
        public SettingsAutomationPeer(FrameworkElement owner)
            : base(owner)
        {

        }

        // Methods
        protected override string GetAutomationIdCore()
        {
            return "Settings";
        }

        protected override string GetClassNameCore()
        {
            return typeof(Settings).Name;
        }

        protected override string GetNameCore()
        {
            return "Settings";
        }
    }
}
