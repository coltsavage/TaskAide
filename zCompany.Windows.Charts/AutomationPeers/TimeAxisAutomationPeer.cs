using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace zCompany.Windows.Charts
{
    internal class TimeAxisAutomationPeer : FrameworkElementAutomationPeer
    {
        public TimeAxisAutomationPeer(FrameworkElement owner)
            : base(owner)
        {

        }

        protected override string GetAutomationIdCore()
        {
            return "TimeAxis";
        }

        protected override string GetClassNameCore()
        {
            return "TimeAxis";
        }

        protected override string GetNameCore()
        {
            return "TimeAxis";
        }
    }
}
