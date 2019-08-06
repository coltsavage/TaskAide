using Windows.UI.Xaml.Controls;

namespace zCompany.TaskAide.WindowsApp
{
    // Not used for navigation: Settings NavigationViewItem implicitly part of NavigationView.
    // Exists simply for consistency across UIAutomation.

    public class SettingsNavigationViewItem : AppNavigationViewItemBase<Settings>
    {
        protected override IconElement IconElement => throw new System.NotImplementedException();
    }
}
