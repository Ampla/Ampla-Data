using System;
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

        /// <summary>
        /// Determines whether this instance [can map field] the specified model properties.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="modelProperties">The model properties.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public override bool CanMapField<TModel>(IModelProperties<TModel> modelProperties, out string message)
        {
            bool canRead = modelProperties.CanConvertTo(FieldType, Name);
            if (!canRead)
            {
                Type propertyType = modelProperties.GetPropertyType(Name);
                message = string.Format("{0}.{1} is not able to be read ({2}) Ampla field type is {3}.", typeof(TModel), Name, FieldType, propertyType);
           
                return false;
            }
            message = null;
            return true;
        }
    }
}