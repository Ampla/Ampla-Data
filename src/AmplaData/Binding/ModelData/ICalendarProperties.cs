using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.Binding.ModelData.Validation;

namespace AmplaData.Binding.ModelData
{
    /// <summary>
    ///     Interface that provides access against a generic model
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface ICalendarProperties<TModel> where TModel : new()
    {
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