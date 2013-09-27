using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.AmplaRepository
{
    /// <summary>
    ///     Ampla Repository Set are used to create Ampla Repositories 
    /// </summary>
    public class AmplaRepositorySet : IRepositorySet
    {
        public AmplaRepositorySet(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }
        
        private readonly string userName;
        private readonly string password;

        /// <summary>
        /// Gets the Ampla repository for the specficied model
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        public IRepository<TModel> GetRepository<TModel>() where TModel : class, new()
        {
            DataWebServiceClient webServiceClient = new DataWebServiceClient("NetTcpBinding_IDataWebService");
            return new AmplaRepository<TModel>(webServiceClient, userName, password);
        }

        /// <summary>
        /// Gets the read only repository for the specified model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        public IReadOnlyRepository<TModel> GetReadOnlyRepository<TModel>() where TModel : class, new()
        {
            DataWebServiceClient webServiceClient = new DataWebServiceClient("NetTcpBinding_IDataWebService");
            return new AmplaReadOnlyRepository<TModel>(webServiceClient, userName, password);
        }
    }
}
