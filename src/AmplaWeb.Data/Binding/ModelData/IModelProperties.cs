using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.ModelData.Validation;

namespace AmplaWeb.Data.Binding.ModelData
{
    /// <summary>
    ///     Interface that provides access against a generic model
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IModelProperties<in TModel> where TModel : new()
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
    }
}