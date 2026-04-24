using System;
using System.Globalization;
using System.Windows.Data;

namespace ShiBoo.Helpers
{
    public class ShiftDisableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] == null || values[1] == null) return true;

            string itemContent = values[0].ToString(); 
            string currentShift = values[1].ToString(); 

            return itemContent != currentShift;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}