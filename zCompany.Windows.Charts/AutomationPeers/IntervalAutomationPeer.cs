using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace zCompany.Windows.Charts
{
    internal class IntervalAutomationPeer : FrameworkElementAutomationPeer
    {
        public IntervalAutomationPeer(FrameworkElement owner)
            : base(owner)
        {

        }

        protected override string GetAutomationIdCore()
        {
            var instance = ((Interval)base.Owner).InstanceNumber;
            return $"Interval#{instance}";
        }

        protected override string GetClassNameCore()
        {
            return "Interval";
        }

        protected override string GetNameCore()
        {
            string name = ((Interval)base.Owner).ViewModel.Name;
            var instance = ((Interval)base.Owner).InstanceNumber;
            return $"{name}#{instance}";
        }
    }
}
