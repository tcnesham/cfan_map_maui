using System;
using System.Globalization;
using System.Linq;
using CFAN.Common.WPF;
using ISO3166;
 
namespace CFAN.SchoolMap.WPF
{
    public class CountryNameVc : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Country)) return "Choose a country";
            var c = (Country) value;
            var name = c.Name.Split(',').FirstOrDefault();
            if (name?.Length > 50) name = name.Substring(0, 50);
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
            //throw new NotImplementedException();
        }
    }

    public class CountryNameCX : ConverterME<CountryNameVc> {}
}
