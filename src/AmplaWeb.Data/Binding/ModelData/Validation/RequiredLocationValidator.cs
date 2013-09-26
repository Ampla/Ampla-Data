using System;

namespace AmplaWeb.Data.Binding.ModelData.Validation
{
    /// <summary>
    ///     Model Validator that requires the location to be a specified value
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class RequiredLocationValidator<TModel> : IModelValidator<TModel> where TModel : class, new()
    {
        private readonly string location;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredLocationValidator{TModel}"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        public RequiredLocationValidator(string location)
        {
            this.location = location;
        }

        /// <summary>
        /// Validates the specified model location is the specified value.
        /// </summary>
        /// <param name="modelProperties">The model properties.</param>
        /// <param name="model">The model.</param>
        /// <param name="validationMessages">The validation messages.</param>
        /// <returns></returns>
        public bool Validate(IModelProperties<TModel> modelProperties, TModel model, ValidationMessages validationMessages)
        {
            string newlocation = modelProperties.GetLocation(model);

            bool isValid = string.Compare(location, newlocation, StringComparison.InvariantCulture) == 0;
            
            if (!isValid)
            {
                validationMessages.Add("The Location property is not the required value: " + location);
            }
            return isValid;
        }
    }
}