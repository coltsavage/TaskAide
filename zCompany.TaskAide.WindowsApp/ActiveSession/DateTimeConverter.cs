using System;
using Windows.UI.Xaml.Data;

namespace zCompany.TaskAide.WindowsApp
{
    internal class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((DateTimeOffset)value).LocalDateTime.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
