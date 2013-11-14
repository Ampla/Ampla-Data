using AmplaData.Binding.ModelData;

namespace AmplaData.Binding.Mapping
{
    /// <summary>
    /// Readonly field mapping 
    /// </summary>
    public class ReadOnlyFieldMapping : FieldMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyFieldMapping"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ReadOnlyFieldMapping(string name) : base(name)
        {
            CanWrite = false;
        }

        /// <summary>
        ///     Don't resolve the value from the model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="modelProperties"></param>
        /// <param name="model"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TryResolveValue<TModel>(IModelProperties<TModel> modelProperties, TModel model, out string value)
        {
            value = null;
            return false;
        }
    }
}