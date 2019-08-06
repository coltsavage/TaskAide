using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace zCompany.Windows.Charts
{
    internal class MajorTickAutomationPeer : FrameworkElementAutomationPeer
    {
        // Constructors
        public MajorTickAutomationPeer(FrameworkElement owner)
            : base(owner)
        {

        }

        // Methods
        protected override string GetAutomationIdCore()
        {
            var instance = ((MajorTick)base.Owner).InstanceNumber;
            return $"MajorTick#{instance}";
        }

        protected override string GetClassNameCore()
        {
            return typeof(MajorTick).Name;
        }

        protected override string GetNameCore()
        {
            var label = ((MajorTick)base.Owner).Label;
            return $"{label}";
        }
    }
}
