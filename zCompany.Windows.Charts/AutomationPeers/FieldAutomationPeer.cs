using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace zCompany.Windows.Charts
{
    internal class FieldAutomationPeer : FrameworkElementAutomationPeer
    {
        public FieldAutomationPeer(FrameworkElement owner)
            : base(owner)
        {

        }

        protected override string GetAutomationIdCore()
        {
            return "Field";
        }

        protected override string GetClassNameCore()
        {
            return "Field";
        }

        protected override string GetNameCore()
        {
            return "Field";
        }
    }
}
