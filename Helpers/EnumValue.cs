using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CFAN.SchoolMap.Helpers
{
    public class EnumValue<TEnum> : IEnumValue
    {
        #region Veřejné vlastnosti
        public string Description => EnumHelper.GetEnumDescription(typeof(TEnum), (Enum)(object)Value);

        public TEnum Value { get; set; }
        public int ValueInt
        {
            get => (int)(object)Value;
            set => Value = (TEnum)(object)value;
        }
        #endregion

        #region Konstruktor
        public EnumValue(TEnum enumValue)
        {
            Value = enumValue;
        }
        #endregion

        #region Veřejné metody
        public static List<EnumValue<TEnum>> GetValues()
        {
            var enumerations = new List<EnumValue<TEnum>>();
            foreach (FieldInfo fieldInfo in typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumerations.Add(new EnumValue<TEnum>((TEnum)fieldInfo.GetValue(null)));
            }
            return enumerations;
        }

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }

    public class EnumValue : IEnumValue
    {
        #region Veřejné vlastnosti
        public Type EnumType { get; private set; }
        public string Description => EnumHelper.GetEnumDescription(EnumType, Value);

        public Enum Value { get; set; }
        public int ValueInt
        {
            get => (int)(object)Value;
            set => Value = (Enum)(object)value;
        }
        #endregion

        #region Konstruktor
        public EnumValue(Type enumType, Enum enumValue)
        {
            EnumType = enumType;
            Value = enumValue;
        }

        #endregion

        #region Veřejné metody
        public static List<EnumValue> GetValues(Type enumType)
        {
            var enumerations = new List<EnumValue>();
            foreach (FieldInfo fieldInfo in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumerations.Add(new EnumValue(enumType, (Enum)fieldInfo.GetValue(null)));
            }
            return enumerations;
        }

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
