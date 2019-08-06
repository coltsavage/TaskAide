using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace zCompany.TaskAide.WindowsApp
{
    public partial class AppNavigation : Page
    {
        // Constructors
        public AppNavigation()
        {
            this.InitializeComponent();
        }

        // Event Handlers
        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs args)
        {
            throw new Exception("Failed to load Page " + args.SourcePageType.FullName);
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs args)
        {
            this.NavView.SelectedItem = this.NavView.MenuItems[0];
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Type page = (args.IsSettingsSelected) ? typeof(Settings) : (Type)args.SelectedItemContainer.Tag;
            this.ContentFrame.Navigate(page);
        }
    }
}
