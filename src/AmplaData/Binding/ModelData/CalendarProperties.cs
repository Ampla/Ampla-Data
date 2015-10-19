using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using AmplaData.AmplaData2008;
using AmplaData.Attributes;
using AmplaData.Binding.MetaData;
using AmplaData.Binding.ModelData.Validation;
using AmplaData.Display;

namespace AmplaData.Binding.ModelData
{
    /// <summary>
    /// Model Properties provides access to the TModel object and the properties. 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class CalendarProperties<TModel> : ICalendarProperties<TModel> where TModel : new()
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarProperties{TModel}"/> class.
        /// </summary>
        public CalendarProperties()
        {
            AmplaModules? temp;
            List<string> properties = new List<string>();
            foreach (PropertyInfo property in typeof(TModel).GetProperties())
            {
                string propertyName;
                AmplaFieldAttribute.TryGetField(property, out propertyName);
                
                properties.Add(propertyName);
                //propertyInfoDictionary[propertyName] = property;
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
                //typeConverterDictionary[propertyName] = typeConverter;

                //AddModelValidators(property, modelValidators);
            }
            //modelValidators.Add(new NullModelValidator<TModel>());
            //propertyNames = properties.ToArray();

            //if (!AmplaDefaultFiltersAttribute.TryGetFilter<TModel>(out defaultFilters))
            {
             //   defaultFilters = new FilterValue[0];
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
        /// Tries to set a string value of the property for the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TrySetValueFromString(TModel model, string propertyName, string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a list of the property names of the model
        /// </summary>
        /// <returns></returns>
        public IList<string> GetProperties()
        {
//            return propertyNames;
            return new List<string>();
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
            throw new NotImplementedException();
        }




        /// <summary>
        /// Determines whether the model's property is currently a default value
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="property">Name of the property.</param>
        /// <returns></returns>
        public bool IsDefaultValue(TModel model, string property)
        {
            throw new NotImplementedException();
        }

        public bool ValidateModel(TModel model, ValidationMessages validationMessages)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clones the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public TModel CloneModel(TModel model)
        {
            throw new NotImplementedException();
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