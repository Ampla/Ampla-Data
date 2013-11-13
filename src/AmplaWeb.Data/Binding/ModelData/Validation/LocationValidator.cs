

namespace AmplaData.Data.Binding.ModelData.Validation
{
    /// <summary>
    ///     Location Validation to check that a Location is required if the model specifies it.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class LocationValidator<TModel> : IModelValidator<TModel> where TModel : new()
    {
        public bool Validate(IModelProperties<TModel> modelProperties, TModel model, ValidationMessages validationMessages)
        {
            string location;

            bool isValid = true;
            if (modelProperties.TryGetPropertyValue(model, "Location", out location))
            {
                isValid = !string.IsNullOrEmpty(location);
                if (!isValid)
                {
                    string message = string.Format("The Location property is not specified. Type='{0}'", typeof (TModel));
                    validationMessages.Add(message);
                }
            }
            return isValid;
        }
    }
}