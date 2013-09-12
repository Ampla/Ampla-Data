using System;
using System.Linq;
using System.Reflection;

namespace AmplaWeb.Data.Binding.MetaData
{
    public static class ReflectionHelper
    {
        public static bool TryGetPropertyByAttribute<T>(Type type, out PropertyInfo property) where T : Attribute
        {
            property = null;
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (propertyInfo.AttributeExists<T>())
                {
                    property = propertyInfo;
                    return true;
                }
            }
            return false;
        }

        public static bool TryGetAttribute<T>(this PropertyInfo propertyInfo, out T attribute) where T : Attribute
        {
            attribute = propertyInfo.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
            return attribute != null;
        }

        public static bool TryGetAttribute<T>(this Type classType, out T attribute) where T : Attribute
        {
            attribute = classType.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
            return attribute != null;
        }

        public static bool AttributeExists<T>(this PropertyInfo propertyInfo) where T : Attribute
        {
            T attribute;
            return TryGetAttribute(propertyInfo, out attribute);
        }

        public static bool TryGetPropertyByName(Type type, string propertyName, StringComparison stringComparison, out PropertyInfo property)
        {
            property = null;
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (string.Compare(propertyName, propertyInfo.Name, stringComparison) == 0)
                {
                    property = propertyInfo;
                    return true;
                }
            }
            return false;
        }

        public static PropertyInfo[] GetProperties(Type type)
        {
            return type.GetProperties();
        }

    }
}