using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace zCompany.Windows.Charts
{
    internal class TimeAxisAutomationPeer : FrameworkElementAutomationPeer
    {
        // Constructors
        public TimeAxisAutomationPeer(FrameworkElement owner)
            : base(owner)
        {

        }

        // Methods
        protected override string GetAutomationIdCore()
        {
            return "TimeAxis";
        }

        protected override string GetClassNameCore()
        {
            return typeof(TimeAxis).Name;
        }

        protected override string GetNameCore()
        {
            return "TimeAxis";
        }
    }
}
