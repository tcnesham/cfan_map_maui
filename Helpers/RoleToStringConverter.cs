using CFAN.SchoolMap.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Maui.Controls;

namespace CFAN.SchoolMap.Helpers
{
    public class RoleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Role role)
            {
                return EnumHelper.GetEnumDescription(role);
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string roleDescription)
            {
                return EnumHelper.GetValues(typeof(Role)).Where(r => (string)Convert(r, null, null, null) == roleDescription).FirstOrDefault();
            }
            else
            {
                throw new InvalidCastException();
            }
        }
    }
}
