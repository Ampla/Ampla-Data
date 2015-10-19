using System;
using AmplaData.Binding.ModelData;

namespace AmplaData.Binding.Mapping
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
        public override bool TryResolveValue<TModel>(IModelProperties<TModel> modelProperties, TModel model, out string value)
        {
            bool resolved = modelProperties.TryGetPropertyValue(model, Name, out value);
            if (resolved && (value == "0"))
            {
                value = null;
                return false;
            }
            return resolved;
        }

        public override bool CanMapField<TModel>(IModelProperties<TModel> modelProperties, out string message)
        {
            Type propertyType = modelProperties.GetPropertyType(Name);
            if (!typeof (int).IsAssignableFrom(propertyType))
            {            
                message = string.Format("{0}.{1} is not a compatible data type ({2}) Ampla field type is {3}.", typeof (TModel), Name, FieldType, typeof(int));
                return false;
            }
            message = null;
            return true;
        }
    }
}