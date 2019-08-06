using Windows.UI.Xaml.Controls;

namespace zCompany.TaskAide.WindowsApp
{
    public class ActiveSessionNavigationViewItem : AppNavigationViewItemBase<ActiveSession>
    {
        // Class Fields
        public new static string EntryName = "Active";
        private static string glyph = "\uE823"; // Recent

        // Constructors
        public ActiveSessionNavigationViewItem() {}

        // Properties
        protected override string ContentString { get => ActiveSessionNavigationViewItem.EntryName; }

        protected override IconElement IconElement { get => base.IconFromSegoeMdl2Assets(glyph); }
    }
}
