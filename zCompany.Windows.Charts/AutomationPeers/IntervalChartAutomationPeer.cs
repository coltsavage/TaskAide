using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace zCompany.Windows.Charts
{
    internal class IntervalChartAutomationPeer : FrameworkElementAutomationPeer
    {
        // Constructors
        public IntervalChartAutomationPeer(FrameworkElement owner)
            : base(owner)
        {

        }

        // Methods
        protected override string GetAutomationIdCore()
        {
            return "Chart";
        }

        protected override string GetClassNameCore()
        {
            return typeof(IntervalChart).Name;
        }

        protected override string GetNameCore()
        {
            return "Chart";
        }
    }
}
