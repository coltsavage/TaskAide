using Windows.UI.Xaml.Controls;

namespace zCompany.TaskAide.WindowsApp
{
    public class ProjectsNavigationViewItem : AppNavigationViewItemBase<Projects>
    {
        // Class Fields
        private static string glyph = "\uE821"; // Work

        // Constructors
        public ProjectsNavigationViewItem() { }

        // Properties
        protected override IconElement IconElement { get => base.IconFromSegoeMdl2Assets(glyph); }
    }
}
