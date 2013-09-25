using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding.Mapping
{
    /// <summary>
    /// Field Mapping for a constant value field
    /// </summary>
    public class ConstantFieldMapping : FieldMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantFieldMapping"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public ConstantFieldMapping(string name, string value) : base(name)
        {
            CanWrite = false;

            Value = value;
        }

        /// <summary>
        /// The value to use for the constant
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; private set; }

        /// <summary>
        /// Resolve the value from the the constant value (rather than the model)
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="modelProperties">The model properties.</param>
        /// <param name="model">The model.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override bool TryResolveValue<TModel>(IModelProperties<TModel> modelProperties, TModel model, out string value)
        {
            value = Value;
            return true;
        }
    }
}