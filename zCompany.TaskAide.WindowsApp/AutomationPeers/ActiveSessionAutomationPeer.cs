using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace zCompany.TaskAide.WindowsApp
{
    internal class ActiveSessionAutomationPeer : FrameworkElementAutomationPeer
    {
        // Constructors
        public ActiveSessionAutomationPeer(FrameworkElement owner)
            : base(owner)
        {

        }

        // Methods
        protected override string GetAutomationIdCore()
        {
            return "ActiveSession";
        }

        protected override string GetClassNameCore()
        {
            return typeof(ActiveSession).Name;
        }

        protected override string GetNameCore()
        {
            return "ActiveSession";
        }
    }
}
