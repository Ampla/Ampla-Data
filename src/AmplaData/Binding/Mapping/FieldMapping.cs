using System;
using AmplaData.Binding.ModelData;

namespace AmplaData.Binding.Mapping
{
    /// <summary>
    ///     Base class that provides a way for mapping values from objects to Ampla webservice fields
    /// </summary>
    public abstract class FieldMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldMapping"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected FieldMapping(string name) : this(name, typeof(string))
        {
        }

        protected FieldMapping(string name, Type fieldType)
        {
            Name = name;
            FieldType = fieldType;
        }

        /// <summary>
        /// The name of the field
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Does the field support writing?
        /// </summary>
        /// <value>
        ///   <c>true</c> if [can write]; otherwise, <c>false</c>.
        /// </value>
        public bool CanWrite
        {
            get; protected set;
        }

        /// <summary>
        /// Gets the type of the Ampla field
        /// </summary>
        /// <value>
        /// The type of the field.
        /// </value>
        public Type FieldType { get; private set; }

        /// <summary>
        /// Try to resolve the value from the model
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="modelProperties">The model properties.</param>
        /// <param name="model">The model.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public abstract bool TryResolveValue<TModel>(IModelProperties<TModel> modelProperties, TModel model, out string value) where TModel : new();

        /// <summary>
        /// Determines whether this instance [can map field] the specified model properties.
        /// </summary>
        /// <param name="modelProperties">The model properties.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public abstract bool CanMapField<TModel>(IModelProperties<TModel> modelProperties, out string message) where TModel : new();
    }
}