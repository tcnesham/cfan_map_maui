using System;
using System.Globalization;


namespace CFAN.Common.WPF
{
    public class NegationVC : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
            //throw new NotImplementedException();
        }
    }

    public class NegationCX:ConverterME<NegationVC>{}
}
