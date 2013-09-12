using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding.Mapping
{
    /// <summary>
    /// Id field mapper that allows will get a non-zero id
    /// </summary>
    public class IdFieldMapping : FieldMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdFieldMapping"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public IdFieldMapping(string name) : base(name)
        {  
        }

        /// <summary>
        /// Try to resolve the value from the model
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="modelProperties">The model properties.</param>
        /// <param name="model">The model.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override bool TryResolveValue<TModel>(ModelProperties<TModel> modelProperties, TModel model, out string value)
        {
            bool resolved = modelProperties.TryGetPropertyValue(model, Name, out value);
            if (resolved && (value == "0"))
            {
                value = null;
                return false;
            }
            return resolved;
        }
    }
}