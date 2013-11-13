namespace AmplaData.Data
{
    /// <summary>
    /// Interface that provides access to generic models
    /// </summary>
    public interface IRepositorySet
    {
        /// <summary>
        /// Gets the repository for the specficied model
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        IRepository<TModel> GetRepository<TModel>() where TModel : class, new();

        /// <summary>
        /// Gets the read only repository for the specified model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        IReadOnlyRepository<TModel> GetReadOnlyRepository<TModel>() where TModel : class, new();
    }
}