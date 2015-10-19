using System;
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
        /// <param name="propertyType"></param>
        public ModelFieldMapping(string name, Type propertyType) : base(name, propertyType)
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

        /// <summary>
        /// Determines whether this instance [can map field] the specified model properties.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="modelProperties">The model properties.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public override bool CanMapField<TModel>(IModelProperties<TModel> modelProperties, out string message)
        {
            Type propertyType = modelProperties.GetPropertyType(Name);

            bool canRead = modelProperties.CanConvertTo(FieldType, Name);
            bool canWrite = CanWrite && modelProperties.CanConvertFrom(FieldType, Name);

            bool canRoundTrip = CanRoundTrip(FieldType, propertyType);

            message = null;
            if (canRead && canWrite && canRoundTrip)
            {
                return true;
            }
            
            string verb = !canRead && !canWrite ?"is not" : "may not be";
            message = string.Format("Model property '{0}' ({1}) {2} compatible with the Ampla field ({3})", Name, propertyType, verb, FieldType);
            return false;
        }

        private static bool CanRoundTrip(Type amplaFieldType, Type propertyType)
        {
            if (amplaFieldType == propertyType)
            {
                return true;
            }

            if (amplaFieldType.IsAssignableFrom(propertyType) && propertyType.IsAssignableFrom(amplaFieldType))
            {
                return true;
            }

            if (amplaFieldType == typeof(string) || propertyType == typeof(string))
            {
                return false;
            }

            if (propertyType == typeof (TimeSpan) && (amplaFieldType == typeof(int) || amplaFieldType == typeof(double)))
            {
                return true;
            }

            return false;
        }

    }
}