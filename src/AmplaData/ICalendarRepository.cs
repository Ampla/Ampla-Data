using System;
using System.Collections.Generic;

namespace AmplaData
{
    public interface ICalendarRepository<TModel> 
    {
        /// <summary>
        /// Get all the models 
        /// </summary>
        /// <returns></returns>
        IList<TModel> GetAll();

        /// <summary>
        ///     Finds a model using the specified dateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        TModel FindForDateTime(DateTime dateTime);

        /// <summary>
        ///     Find a list of models that match the specified filters
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        IList<TModel> FindByFilter(params FilterValue[] filters);

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

    }
}