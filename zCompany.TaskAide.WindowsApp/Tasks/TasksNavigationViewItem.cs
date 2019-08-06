using Windows.UI.Xaml.Controls;

namespace zCompany.TaskAide.WindowsApp
{
    public class TasksNavigationViewItem : AppNavigationViewItemBase<Tasks>
    {
        // Class Fields
        private static string glyph = "\uE762"; // MultiSelect

        // Constructors
        public TasksNavigationViewItem() {}

        // Properties
        protected override IconElement IconElement { get => base.IconFromSegoeMdl2Assets(glyph); }
    }
}
