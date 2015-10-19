using System;
using AmplaData.Binding.ModelData;

namespace AmplaData.Binding.Mapping
{
    /// <summary>
    ///     Field Mapping for required Ampla fields that are not mapped to the model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequiredFieldMapping<T> : FieldMapping
    {
        private readonly Func<string> defaultValue;

        public RequiredFieldMapping(string name, Func<string> defaultValue) : base(name, typeof(T))
        {
            this.defaultValue = defaultValue;
            CanWrite = true;
        }

        /// <summary>
        /// Try to resolve the value from the model
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="modelProperties">The model properties.</param>
        /// <param name="model">The model.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override bool TryResolveValue<TModel>(IModelProperties<TModel> modelProperties, TModel model, out string value)
        {
            value = defaultValue();
            return true;
        }

        /// <summary>
        /// Determines whether this instance [can map field] the specified model properties.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="modelProperties">The model properties.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public override bool CanMapField<TModel>(IModelProperties<TModel> modelProperties, out string message)
        {
            message = null;
            return true;
        }
    }
}