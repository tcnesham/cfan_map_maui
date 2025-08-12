using System;
using System.Globalization;


namespace CFAN.Common.WPF
{
    public class NegationVC : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return !(value is bool b && b);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b;
            return false;
            //throw new NotImplementedException();
        }
    }

    public class NegationCX:ConverterME<NegationVC>{}
}
