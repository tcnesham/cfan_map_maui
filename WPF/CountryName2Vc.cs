using System.Globalization;
using CFAN.Common.WPF;
using ISO3166;

namespace CFAN.SchoolMap.Maui.WPF
{
    public class CountryName2Vc : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Country? c = value as Country;
            if (c==null || !c.Name.Contains(',')) return "";
            var beginning = c.Name.Split(',').LastOrDefault();
            var end = c.Name.Split(',').FirstOrDefault();
            return beginning + " " + end;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value ?? "";
        }
    }

    public class CountryName2CX : ConverterME<CountryName2Vc> {}
}
