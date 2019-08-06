using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace zCompany.Windows.Charts
{
    internal class FieldAutomationPeer : FrameworkElementAutomationPeer
    {
        // Constructors
        public FieldAutomationPeer(FrameworkElement owner)
            : base(owner)
        {

        }

        // Methods
        protected override string GetAutomationIdCore()
        {
            return "Field";
        }

        protected override string GetClassNameCore()
        {
            return typeof(Field).Name;
        }

        protected override string GetNameCore()
        {
            return "Field";
        }
    }
}
