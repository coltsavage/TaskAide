using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace zCompany.TaskAide.WindowsApp
{
    public abstract class AppNavigationViewItemBase<T> : NavigationViewItem
    {
        // Class Fields
        public static string EntryName = typeof(T).Name;

        // Constructors
        public AppNavigationViewItemBase()
        {
            base.Content = this.ContentString;
            base.Icon = this.IconElement;
            base.Tag = typeof(T);
        }

        // Properties
        protected virtual string ContentString { get => AppNavigationViewItemBase<T>.EntryName; }

        protected abstract IconElement IconElement { get; }
        
        // Methods
        protected FontIcon IconFromSegoeMdl2Assets(string glyph)
        {
            var icon = new FontIcon();
            icon.FontFamily = new FontFamily("Segoe MDL2 Assets");
            icon.Glyph = glyph;
            return icon;
        }

        protected SymbolIcon IconFromSymbolEnum(Symbol symbol)
        {
            return new SymbolIcon(symbol);
        }
    }
}
