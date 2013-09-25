using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding.Mapping
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
        protected FieldMapping(string name)
        {
            Name = name;
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
        /// Try to resolve the value from the model
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="modelProperties">The model properties.</param>
        /// <param name="model">The model.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public abstract bool TryResolveValue<TModel>(IModelProperties<TModel> modelProperties, TModel model, out string value) where TModel : new();
    }
}