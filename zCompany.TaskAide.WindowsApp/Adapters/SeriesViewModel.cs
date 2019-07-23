using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI;
using zCompany.Windows.Charts;

namespace zCompany.TaskAide.WindowsApp
{
    internal class SeriesViewModel : ISeriesViewModel
    {
        // Constructors
        public SeriesViewModel(string name, Color color)
        {
            this.Name = name;
            this.Color = color;
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        private Color color;
        public Color Color
        {
            get => this.color;
            set
            {
                this.color = value;
                this.NotifyPropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.NotifyPropertyChanged();
            }
        }

        // Helpers
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
