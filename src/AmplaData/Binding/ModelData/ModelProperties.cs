using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using AmplaData.AmplaData2008;
using AmplaData.Attributes;
using AmplaData.Binding.Mapping;
using AmplaData.Binding.MetaData;
using AmplaData.Binding.ModelData.Validation;
using AmplaData.Display;

namespace AmplaData.Binding.ModelData
{
    /// <summary>
    /// Model Properties provides access to the TModel object and the properties. 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class ModelProperties<TModel> : IModelProperties<TModel> where TModel : new()
    {
        private readonly Dictionary<string, PropertyInfo> propertyInfoDictionary = new Dictionary<string, PropertyInfo>();
        private readonly Dictionary<string, TypeConverter> typeConverterDictionary = new Dictionary<string, TypeConverter>();
        private readonly List<IModelValidator<TModel>> modelValidators = new List<IModelValidator<TModel>>();  
        private readonly string[] propertyNames;
        private readonly LocationFilter locationFilter;
        private readonly AmplaModules module;
        private readonly FilterValue[] defaultFilters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelProperties{TModel}"/> class.
        /// </summary>
        public ModelProperties()
        {
            AmplaModules? temp;
            bool ok = AmplaLocationAttribute.TryGetLocation<TModel>(out locationFilter);
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

                AddModelValidators(property, modelValidators);
            }

            modelValidators.Add(new NullModelValidator<TModel>());
            propertyNames = properties.ToArray();

            if (!AmplaDefaultFiltersAttribute.TryGetFilter<TModel>(out defaultFilters))
            {
                defaultFilters = new FilterValue[0];
            }
        }

        private static void AddModelValidators(PropertyInfo property, IList<IModelValidator<TModel>> validators )
        {
            if (property.Name == "Location")
            {
                validators.Add(new LocationValidator<TModel>());
            }
        }

        private static TypeConverter GetTypeConverter(PropertyInfo property)
        {
            if (property.PropertyType == typeof (DateTime))
            {
                return new Iso8601DateTimeConverter();
            }

            if (property.PropertyType == typeof (TimeSpan))
            {
                return new TimeSpanIntConverter();
            }

            if (property.PropertyType == typeof (int))
            {
                return new AllowEmptyStringConverter<int>(0);
            }

            if (property.PropertyType == typeof (double))
            {
                return new AllowEmptyStringConverter<double>(0d);
            }

            if (property.PropertyType == typeof(float))
            {
                return new AllowEmptyStringConverter<float>(0f);
            }

            if (property.PropertyType == typeof(long))
            {
                return new AllowEmptyStringConverter<long>(0);
            }

            if (property.PropertyType == typeof(bool))
            {
                return new AllowEmptyStringConverter<bool>(false);
            }

            return TypeDescriptor.GetConverter(property.PropertyType);
        }

        /// <summary>
        /// Gets the location from the model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public string GetLocation(TModel model)
        {
            string modelLocation = null;
            if (!Equals(model, null))
            {
                TryGetPropertyValue(model, "Location", out modelLocation);
            }
            if (string.IsNullOrEmpty(modelLocation))
            {
                return locationFilter.WithRecurse ? null : locationFilter.Location;
            }
            return modelLocation;
        }

        /// <summary>
        ///     The Ampla Location that the model represents
        /// </summary>
        public LocationFilter LocationFilter
        {
            get { return locationFilter; }
        }

        public FilterValue[] DefaultFilters { get { return defaultFilters; } }

        /// <summary>
        ///     The Ampla module 
        /// </summary>
        public AmplaModules Module
        {
            get { return module; }
        }

        public bool ResolveIdentifiers
        {
            get
            {
                bool requiresId = false;
                bool requiresString = false;

                PropertyInfo cause;
                PropertyInfo classification;
                propertyInfoDictionary.TryGetValue("Cause", out cause);
                propertyInfoDictionary.TryGetValue("Classification", out classification);

                if (cause != null)
                {
                    requiresId |= cause.PropertyType == typeof (int);
                    requiresString |= cause.PropertyType == typeof (string);
                }

                if (classification != null)
                {
                    requiresId |= classification.PropertyType == typeof (int);
                    requiresString |= classification.PropertyType == typeof(string);
                }
                return !requiresId || requiresString;
            }
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
        /// Determines whether the property the specified property type.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public bool CanConvertTo(Type propertyType, string propertyName)
        {
            TypeConverter converter;
            if (typeConverterDictionary.TryGetValue(propertyName, out converter))
            {
                return converter.CanConvertTo(propertyType);
            }
            return false;
        }

        /// <summary>
        /// Determines whether this instance can write the specified field mapping.
        /// </summary>
        /// <param name="fieldMapping">The field mapping.</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool CanMapField(FieldMapping fieldMapping, out string message)
        {
            return fieldMapping.CanMapField(this, out message);
        }

        public bool CanRoundTrip(Type fieldType, string propertyName)
        {
            TypeConverter converter;
            if (typeConverterDictionary.TryGetValue(propertyName, out converter))
            {


                return true;
            }
            return false;
        }

        public bool CanConvertFrom(Type propertyType, string propertyName)
        {
            TypeConverter converter;
            if (typeConverterDictionary.TryGetValue(propertyName, out converter))
            {
                return converter.CanConvertFrom(propertyType);
            }
            return false;
        }

        /// <summary>
        /// Gets the data type of the specified model property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public Type GetPropertyType(string propertyName)
        {
            PropertyInfo property;

            if (propertyInfoDictionary.TryGetValue(propertyName, out property))
            {
                return property.PropertyType;
            }
            return null;
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

        public bool ValidateModel(TModel model, ValidationMessages validationMessages)
        {
            return modelValidators.Aggregate(true, (current, validator) => current & validator.Validate(this, model, validationMessages));
        }

        /// <summary>
        /// Clones the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public TModel CloneModel(TModel model)
        {
            TModel clone = new TModel();
            foreach (string propertyName in propertyNames)
            {
                string value;
                if (TryGetPropertyValue(model, propertyName, out value))
                {
                    TrySetValueFromString(clone, propertyName, value);
                }
            }
            return clone;
        }

        public string GetModelName()
        {
            DisplayNameAttribute displayName;
            string name = typeof (TModel).TryGetAttribute(out displayName) 
                ? displayName.DisplayName 
                : typeof (TModel).Name.ToSeparatedWords();
            return name;
        }

        
    }
}