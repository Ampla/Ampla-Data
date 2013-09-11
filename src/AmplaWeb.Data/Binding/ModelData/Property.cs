using System;
using System.Reflection;
using AmplaWeb.Data.Binding.MetaData;

namespace AmplaWeb.Data.Binding.ModelData
{
    public static class Property<TModel>
    {
        public static T GetValue<T>(TModel model, string property)
        {
            if (!string.IsNullOrEmpty(property))
            {
                PropertyInfo propertyInfo;
                if (ReflectionHelper.TryGetPropertyByName(model.GetType(), property, StringComparison.CurrentCulture, out propertyInfo))
                {
                    return (T)propertyInfo.GetValue(model, null);
                }
            }
            return default(T);
        }

        public static void SetValue<T>(TModel model, string property, T value)
        {
            if (!string.IsNullOrEmpty(property))
            {
                PropertyInfo propertyInfo;
                if (ReflectionHelper.TryGetPropertyByName(model.GetType(), property, StringComparison.CurrentCulture, out propertyInfo))
                {
                    propertyInfo.SetValue(model, value, null);
                }
            }
        }

    }
}