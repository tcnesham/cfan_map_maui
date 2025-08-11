using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace CFAN.SchoolMap.Helpers
{
    public static class EnumHelper
    {

        public static string GetEnumDescription(Type enumType, Enum enumValue)
        {
            try
            {

                MemberInfo enumValueInfo = enumType.GetMember(enumValue.ToString())[0];

                var attribute = enumValueInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attribute.Length > 0
                    ? ((DescriptionAttribute)attribute[0]).Description
                    : Enum.ToObject(enumType, enumValue).ToString().Replace("_", " "));
            }
            catch (Exception)
            {
                return $"Neznámý ({(int)(object)enumValue})";
            }

        }

        public static string GetEnumDescription(Enum enumValue)
        {
            return GetEnumDescription(enumValue.GetType(), enumValue);
        }

        public static List<IEnumValue> GetValues(Type enumType)
        {
            var enumVraperType = typeof(EnumValue<>).MakeGenericType(enumType);
            var enumerations = new List<IEnumValue>();
            foreach (FieldInfo fieldInfo in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumerations.Add((IEnumValue)Activator.CreateInstance(enumVraperType, fieldInfo.GetValue(null)));
            }
            return enumerations;
        }

        public static IEnumerable<EnumValue<T>> GetValues<T>()
            where T : struct
        {
            foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                yield return new EnumValue<T>((T)fieldInfo.GetValue(null));
            }
        }

        public static T Parse<T>(string enumName)
        {
            return (T)Enum.Parse(typeof(T), enumName);
        }
    }
}
