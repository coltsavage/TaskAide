using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace zCompany.TaskAide.WindowsApp
{
    internal class DevTimeBarAutomationPeer : FrameworkElementAutomationPeer
    {
        // Constructors
        public DevTimeBarAutomationPeer(FrameworkElement owner)
            : base(owner)
        {

        }

        // Methods
        protected override string GetAutomationIdCore()
        {
            return "DevTimeBar";
        }

        protected override string GetClassNameCore()
        {
            return typeof(DevTimeBar).Name;
        }

        protected override string GetNameCore()
        {
            return "DevTimeBar";
        }
    }
}
