using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AmplaWeb.Data.Binding.MetaData;

namespace AmplaWeb.Data.Binding.ModelData
{
    public static class ModelIdentifier
    {
        private static readonly Dictionary<Type, string> CachedPropertyName = new Dictionary<Type, string>();

         public static string GetPropertyName<TModel>()
         {
             return GetPropertyName(typeof(TModel));
         }

        public static PropertyInfo GetProperty<TModel>()
        {            
            string property = GetPropertyName<TModel>();
            if (!string.IsNullOrEmpty(property))
            {
                PropertyInfo propertyInfo;
                if (ReflectionHelper.TryGetPropertyByName(typeof(TModel), property, StringComparison.CurrentCulture,
                                                          out propertyInfo))
                {
                    return propertyInfo;
                }
            }

            return null;
        }
            
         public static string GetPropertyName(Type type)
         {
             string propertyName;
             if (!CachedPropertyName.TryGetValue(type, out propertyName))
             {
                 PropertyInfo propertyInfo;
                 if (ReflectionHelper.TryGetPropertyByAttribute<KeyAttribute>(type, out propertyInfo))
                 {
                     propertyName = propertyInfo.Name;
                 }
                 else if (ReflectionHelper.TryGetPropertyByName(type, "id", StringComparison.CurrentCultureIgnoreCase,
                                                                out propertyInfo))
                 {
                     propertyName = propertyInfo.Name;
                 }
                 else
                 {
                     propertyName = "";
                 }
                 CachedPropertyName[type] = propertyName;
             }
             return propertyName;
         }

        public static string GetPropertyName(object model)
        {
            return GetPropertyName(model.GetType());
        }

        public static T GetValue<TModel, T>(TModel model)
        {
            PropertyInfo propertyInfo = GetProperty<TModel>();
            return propertyInfo != null ? (T)Convert.ChangeType(propertyInfo.GetValue(model, null), typeof(T)) : default(T);
        }

        public static void SetValue<TModel, T>(TModel model, T value)
        {
            string property = GetPropertyName(model);
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