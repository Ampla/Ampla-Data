using System.Collections.Generic;

namespace AmplaWeb.Data.Binding.ModelData.Validation
{
    /// <summary>
    ///     Validates if the model is ever null
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class NullModelValidator<TModel> : IModelValidator<TModel> where TModel : new()
    {
        /// <summary>
        /// Validates the specified model is not null
        /// </summary>
        /// <param name="modelProperties">The model properties.</param>
        /// <param name="model">The model.</param>
        /// <param name="validationMessages">The validation messages.</param>
        /// <returns></returns>
        public bool Validate(IModelProperties<TModel> modelProperties, TModel model, ValidationMessages validationMessages)
        {
            bool isValid = !Equals(model, null);
            
            if (!isValid)
            {
                string message = string.Format("Null model specified. Type='{0}'", typeof (TModel));
                validationMessages.Add(message);
            }
            return isValid;
        }
    }
}