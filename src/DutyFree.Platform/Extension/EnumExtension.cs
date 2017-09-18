namespace DutyFree.Platform.Extension
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    public static class EnumExtension
    {
        public static string GetDescription(this Enum e)
        {
            Type t = e.GetType();
            string name = Enum.GetName(t, e);
            FieldInfo fieldInfo = t.GetField(name);
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : name;
        }
    }
}
