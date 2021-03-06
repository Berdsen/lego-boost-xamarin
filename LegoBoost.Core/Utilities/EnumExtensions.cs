using System;
using System.Reflection;

namespace LegoBoost.Core.Utilities
{
    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum enumValue)
        {
            Type type = enumValue.GetType();
            FieldInfo fieldInfo = type.GetField(enumValue.ToString());

            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            return attribs?.Length > 0 ? attribs[0].StringValue : "";
        }

    }
}