using Windows.UI.Xaml.Controls;

namespace zCompany.TaskAide.WindowsApp
{
    public class SessionsNavigationViewItem : AppNavigationViewItemBase<Sessions>
    {
        // Class Fields
        private static string glyph = "\uE81E"; // MapLayers

        // Constructors
        public SessionsNavigationViewItem() { }

        // Properties
        protected override IconElement IconElement { get => base.IconFromSegoeMdl2Assets(glyph); }
    }
}
