using CFAN.SchoolMap.Model;
using System.Globalization;

namespace CFAN.SchoolMap.Helpers
{
    public class RoleToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Role role)
            {
                return EnumHelper.GetEnumDescription(role);
            }
            else
            {
                return null;
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string roleDescription)
            {
                var match = Enum.GetValues(typeof(Role))
                    .Cast<Role>()
                    .FirstOrDefault(r => (string)Convert(r, typeof(string), parameter, culture) == roleDescription);

                if (EqualityComparer<Role>.Default.Equals(match, default(Role)))
                {
                    throw new InvalidOperationException($"No matching Role found for description '{roleDescription}'.");
                }
                return match;
            }
            else
            {
                return null;
            }
        }
    }
}
