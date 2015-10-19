using System;
using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.Binding.Mapping;
using AmplaData.Binding.ModelData.Validation;

namespace AmplaData.Binding.ModelData
{
    /// <summary>
    ///     Interface that provides access against a generic model
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IModelProperties<TModel> where TModel : new()
    {
        /// <summary>
        /// Gets the location for the model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        string GetLocation(TModel model);

        /// <summary>
        ///     The Location Filter for accessing the model
        /// </summary>
        LocationFilter LocationFilter { get; }

        /// <summary>
        /// Gets the default filters for the model
        /// </summary>
        /// <value>
        /// The default filters
        /// </value>
        FilterValue[] DefaultFilters { get; }

        /// <summary>
        ///     The Ampla module 
        /// </summary>
        AmplaModules Module { get; }

        /// <summary>
        /// Gets a value indicating whether [resolve identifiers] should be specified
        /// </summary>
        /// <value>
        ///   <c>true</c> if [resolve identifiers]; otherwise, <c>false</c>.
        /// </value>
        bool ResolveIdentifiers { get; }

        /// <summary>
        /// Tries to set a string value of the property for the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        bool TrySetValueFromString(TModel model, string propertyName, string value);

        /// <summary>
        /// Gets a list of the property names of the model
        /// </summary>
        /// <returns></returns>
        IList<string> GetProperties();

        /// <summary>
        /// Determines whether the property can write to specified type.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        bool CanConvertTo(Type propertyType, string propertyName);

        /// <summary>
        /// Determines whether the property can convert from the specified type.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        bool CanConvertFrom(Type propertyType, string propertyName);

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        Type GetPropertyType(string propertyName);

        /// <summary>
        /// Determines whether this instance can write the specified field mapping.
        /// </summary>
        /// <param name="fieldMapping">The field mapping.</param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool CanMapField(FieldMapping fieldMapping, out string message);
        
        /// <summary>
        /// Determines whether the specified field can round trip to and from the specified type
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="propertyName">The name.</param>
        /// <returns></returns>
        bool CanRoundTrip(Type fieldType, string propertyName);

        /// <summary>
        /// Try to get the value of the property from the model as a string
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        bool TryGetPropertyValue(TModel model, string propertyName, out string value);

        /// <summary>
        /// Determines whether the model's property is currently a default value
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        bool IsDefaultValue(TModel model, string propertyName);

        /// <summary>
        ///     Validates the model and adds the messages
        /// </summary>
        /// <param name="model"></param>
        /// <param name="validationMessages"></param>
        /// <returns></returns>
        bool ValidateModel(TModel model, ValidationMessages validationMessages);

        /// <summary>
        /// Clones the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        TModel CloneModel(TModel model);

        /// <summary>
        /// Gets the model name
        /// </summary>
        /// <returns></returns>
        string GetModelName();


    }
}