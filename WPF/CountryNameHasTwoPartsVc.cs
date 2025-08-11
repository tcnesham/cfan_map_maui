using System;
using System.Globalization;
using System.Linq;
using CFAN.Common.WPF;
using ISO3166;

 
namespace CFAN.SchoolMap.WPF
{
    public class CountryNameHasTwoPartsVc : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Country c && c.Name.Contains(',');
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
            //throw new NotImplementedException();
        }
    }
    public class CountryNameHasTwoPartsCX : ConverterME<CountryNameHasTwoPartsVc> {}
}
