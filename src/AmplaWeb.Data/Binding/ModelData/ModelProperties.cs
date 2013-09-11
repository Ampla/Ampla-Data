using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Binding.MetaData;

namespace AmplaWeb.Data.Binding.ModelData
{
    public class ModelProperties<TModel> where TModel : new()
    {
        private readonly Dictionary<string, PropertyInfo> propertyInfoDictionary = new Dictionary<string, PropertyInfo>();
        private readonly Dictionary<string, TypeConverter> typeConverterDictionary = new Dictionary<string, TypeConverter>();
        private readonly string[] propertyNames;
        private readonly string location;
        private readonly AmplaModules module;

        public ModelProperties()
        {
            bool ok = AmplaLocationAttribute.TryGetLocation<TModel>(out location);
            ok &= AmplaModuleAttribute.TryGetModule<TModel>(out module);

            if (!ok) throw new ArgumentException("Unable to read the AmplaLocationAttribute or AmplaModuleAttribute on type: " + typeof(TModel).FullName);

            List<string> properties = new List<string>();
            foreach (PropertyInfo property in typeof(TModel).GetProperties())
            {
                string propertyName = property.Name;
                properties.Add(propertyName);
                propertyInfoDictionary[propertyName] = property;
                TypeConverterAttribute typeConverterAttribute;
                TypeConverter typeConverter = TypeDescriptor.GetConverter(property.PropertyType);
                if (property.TryGetAttribute(out typeConverterAttribute))
                {
                    Type converterType = Type.GetType(typeConverterAttribute.ConverterTypeName);
                    if (converterType != null)
                    {
                        typeConverter = Activator.CreateInstance(converterType) as TypeConverter;
                    }
                }
                typeConverterDictionary[propertyName] = typeConverter;

            }
            propertyNames = properties.ToArray();
        }

        public string Location
        {
            get { return location; }
        }

        public AmplaModules Module
        {
            get { return module; }
        }

        public bool TrySetValueFromString(TModel model, string propertyName, string value)
        {
            PropertyInfo property;
            
            if (propertyInfoDictionary.TryGetValue(propertyName, out property))
            {
                if (property.CanWrite)
                {
                    object propertyValue = ConvertFromString(propertyName, value);
                    property.SetValue(model, propertyValue, null);
                    return true;
                }
            }
            return false;
        }

        private object ConvertFromString(string propertyName, string value)
        {
            TypeConverter converter;

            return typeConverterDictionary.TryGetValue(propertyName, out converter) 
                ? converter.ConvertFromInvariantString(value) 
                : value;
        }

        private string ConvertToString(string propertyName, object value)
        {
            TypeConverter converter;

            return typeConverterDictionary.TryGetValue(propertyName, out converter)
                ? converter.ConvertToInvariantString(value)
                : null;
        }

        public IEnumerable<string> GetProperties()
        {
            return propertyNames;
        }

        public bool IsDefaultValue(TModel model, string propertyName)
        {
            PropertyInfo property;
            if (propertyInfoDictionary.TryGetValue(propertyName, out property))
            {
                TModel defaultModel = new TModel();
                
                object defaultProperty = property.GetValue(defaultModel, null);
                object currentValue = property.GetValue(model, null);

                return Equals(currentValue, defaultProperty);
            }
            return false;
        }

        public bool TryGetPropertyValue(object model, string propertyName, out string value)
        {
            PropertyInfo property;

            if (propertyInfoDictionary.TryGetValue(propertyName, out property))
            {
                if (property.CanRead)
                {
                    object propertyValue = property.GetValue(model, null);
                    value = ConvertToString(propertyName, propertyValue);
                    return true;
                }
            }
            value = null;
            return false;
        }
    }
}