using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace zCompany.Windows.Charts
{
    internal class IntervalChartAutomationPeer : FrameworkElementAutomationPeer
    {
        public IntervalChartAutomationPeer(FrameworkElement owner)
            : base(owner)
        {

        }

        protected override string GetAutomationIdCore()
        {
            return "Chart";
        }

        protected override string GetClassNameCore()
        {
            return "IntervalChart";
        }

        protected override string GetNameCore()
        {
            return "Chart";
        }
    }
}
