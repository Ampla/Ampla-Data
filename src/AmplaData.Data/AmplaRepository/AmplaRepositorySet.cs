using AmplaData.AmplaData2008;

namespace AmplaData.AmplaRepository
{
    /// <summary>
    ///     Ampla Repository Set are used to create Ampla Repositories 
    /// </summary>
    public class AmplaRepositorySet : IRepositorySet
    {
        private readonly ICredentialsProvider credentialsProvider;

        public AmplaRepositorySet(ICredentialsProvider credentialsProvider)
        {
            this.credentialsProvider = credentialsProvider;
        }

        /// <summary>
        /// Gets the Ampla repository for the specficied model
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        public IRepository<TModel> GetRepository<TModel>() where TModel : class, new()
        {
            IDataWebServiceClient webServiceClient = DataWebServiceFactory.Create();
            return new AmplaRepository<TModel>(webServiceClient, credentialsProvider);
        }

        /// <summary>
        /// Gets the read only repository for the specified model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        public IReadOnlyRepository<TModel> GetReadOnlyRepository<TModel>() where TModel : class, new()
        {
            IDataWebServiceClient webServiceClient = DataWebServiceFactory.Create();
            return new AmplaReadOnlyRepository<TModel>(new AmplaRepository<TModel>(webServiceClient, credentialsProvider));
        }
    }
}
