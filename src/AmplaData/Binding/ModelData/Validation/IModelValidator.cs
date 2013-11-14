namespace AmplaData.Binding.ModelData.Validation
{
    public interface IModelValidator<TModel> where TModel : new()
    {
        bool Validate(IModelProperties<TModel> modelProperties, TModel model, ValidationMessages validationMessages);
    }
}