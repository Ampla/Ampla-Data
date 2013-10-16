using System;
using System.Collections.Generic;

namespace AmplaWeb.Data
{
    /// <summary>
    /// Interface that provides read only access to the repository
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IReadOnlyRepository<TModel> : IDisposable
    {
        /// <summary>
        /// Get all the models 
        /// </summary>
        /// <returns></returns>
        IList<TModel> GetAll();

        /// <summary>
        ///     Finds a model using the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TModel FindById(int id);

        /// <summary>
        /// Finds the record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        dynamic FindRecord(int id);

        /// <summary>
        ///     Find a list of models that match the specified filters
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        IList<TModel> FindByFilter(params FilterValue[] filters);
    }
}