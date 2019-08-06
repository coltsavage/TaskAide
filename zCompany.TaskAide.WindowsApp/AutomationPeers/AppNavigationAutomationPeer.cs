using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace zCompany.TaskAide.WindowsApp
{
    internal class AppNavigationAutomationPeer : FrameworkElementAutomationPeer
    {
        // Constructors
        public AppNavigationAutomationPeer(FrameworkElement owner)
            : base(owner)
        {

        }

        // Methods
        protected override string GetAutomationIdCore()
        {
            return "Navigation";
        }

        protected override string GetClassNameCore()
        {
            return typeof(AppNavigation).Name;
        }

        protected override string GetNameCore()
        {
            return "Navigation";
        }
    }
}
