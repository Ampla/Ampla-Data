using AmplaData.Binding.ModelData;

namespace AmplaData.Binding.Mapping
{
    /// <summary>
    /// Model Field Mapping that will represent the field to be mapped
    /// </summary>
    public class ModelFieldMapping : FieldMapping
    {
        /// <summary>
        ///     Creates a new Field Mapping
        /// </summary>
        /// <param name="name"></param>
        public ModelFieldMapping(string name) : base(name)
        {
            CanWrite = true;
        }

        /// <summary>
        /// Try to resolve the value from the model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="modelProperties"></param>
        /// <param name="model"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TryResolveValue<TModel>(IModelProperties<TModel> modelProperties, TModel model, out string value)
        {
            return modelProperties.TryGetPropertyValue(model, Name, out value);
        }
    }
}