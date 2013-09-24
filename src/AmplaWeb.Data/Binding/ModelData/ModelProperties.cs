using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Binding.MetaData;

namespace AmplaWeb.Data.Binding.ModelData
{
    /// <summary>
    /// Model Properties provides access to the TModel object and the properties. 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class ModelProperties<TModel> : IModelProperties<TModel> where TModel : new()
    {
        private readonly Dictionary<string, PropertyInfo> propertyInfoDictionary = new Dictionary<string, PropertyInfo>();
        private readonly Dictionary<string, TypeConverter> typeConverterDictionary = new Dictionary<string, TypeConverter>();
        private readonly string[] propertyNames;
        private readonly string location;
        private readonly AmplaModules module;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelProperties{TModel}"/> class.
        /// </summary>
        public ModelProperties()
        {
            AmplaModules? temp;
            bool ok = AmplaLocationAttribute.TryGetLocation<TModel>(out location);
            ok &= AmplaModuleAttribute.TryGetModule<TModel>(out temp);

            if (!ok) throw new ArgumentException("Unable to read the AmplaLocationAttribute or AmplaModuleAttribute on type: " + typeof(TModel).FullName);

            module = temp ?? AmplaModules.Downtime;

            List<string> properties = new List<string>();
            foreach (PropertyInfo property in typeof(TModel).GetProperties())
            {
                string propertyName;
                AmplaFieldAttribute.TryGetField(property, out propertyName);
                
                properties.Add(propertyName);
                propertyInfoDictionary[propertyName] = property;
                TypeConverterAttribute typeConverterAttribute;
                TypeConverter typeConverter = GetTypeConverter(property);
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

        private static TypeConverter GetTypeConverter(PropertyInfo property)
        {
            if (property.PropertyType == typeof (DateTime))
            {
                return new Iso8601DateTimeConverter();
            }
            return TypeDescriptor.GetConverter(property.PropertyType);
        }

        /// <summary>
        ///     The Ampla Location that the model represents
        /// </summary>
        public string Location
        {
            get { return location; }
        }

        /// <summary>
        ///     The Ampla module 
        /// </summary>
        public AmplaModules Module
        {
            get { return module; }
        }

        /// <summary>
        /// Tries to set a string value of the property for the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TrySetValueFromString(TModel model, string propertyName, string value)
        {
            PropertyInfo property;
            
            if (propertyInfoDictionary.TryGetValue(propertyName, out property))
            {
                if (property.CanWrite)
                {
                    object propertyValue;
                    if (TryConvertFromString(property.PropertyType, propertyName, value, out propertyValue))
                    {
                        property.SetValue(model, propertyValue, null);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a list of the property names of the model
        /// </summary>
        /// <returns></returns>
        public IList<string> GetProperties()
        {
            return propertyNames;
        }

        /// <summary>
        /// Try to get the value of the property from the model as a string
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetPropertyValue(TModel model, string propertyName, out string value)
        {
            PropertyInfo property;

            if (propertyInfoDictionary.TryGetValue(propertyName, out property))
            {
                if (property.CanRead)
                {
                    var propertyValue = property.GetValue(model, null);
                    if (TryConvertToString(property.PropertyType, propertyName, propertyValue, out value))
                    {
                        return true;
                    }
                }
            }
            value = null;
            return false;
        }

        private bool TryConvertFromString(Type type, string propertyName, string stringValue, out object value)
        {
            TypeConverter converter;
            if (typeConverterDictionary.TryGetValue(propertyName, out converter))
            {
                if (stringValue == null)
                {
                    value = null;
                    return !type.IsValueType;
                }
                value = converter.ConvertFromInvariantString(stringValue);
                return true;
            }
            value = null;
            return false;
        }

        private bool TryConvertToString(Type type, string propertyName, object propertyValue, out string value)
        {
            TypeConverter converter;
            if (typeConverterDictionary.TryGetValue(propertyName, out converter))
            {
                if (propertyValue == null)
                {
                    value = null;
                    return !type.IsValueType;
                }
                value = converter.ConvertToInvariantString(propertyValue);
                return true;
            }
            value = null;
            return false;
        }

        /// <summary>
        /// Determines whether the model's property is currently a default value
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="property">Name of the property.</param>
        /// <returns></returns>
        public bool IsDefaultValue(TModel model, string property)
        {
            TModel defaultModel = new TModel();
            string defaultValue;

            string modelValue;

            bool defaultResolved = TryGetPropertyValue(defaultModel, property, out defaultValue);
            bool modelResolved = TryGetPropertyValue(model, property, out modelValue);

            return defaultResolved && modelResolved && (defaultValue == modelValue);
        }
    }
}