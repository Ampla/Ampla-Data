
using System.Collections.Generic;

namespace AmplaData
{
    /// <summary>
    ///     Repository interface that provides access to the underlying models
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IRepository<TModel> : IReadOnlyRepository<TModel>
    {
        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Add(TModel model);

        /// <summary>
        /// Deletes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Delete(TModel model);

        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Update(TModel model);

        /// <summary>
        /// Confirms the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Confirm(TModel model);

        /// <summary>
        /// Unconfirms the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Unconfirm(TModel model);

        /// <summary>
        /// Gets the allowed values for the property
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        List<string> GetAllowedValues(string property);
    }
}