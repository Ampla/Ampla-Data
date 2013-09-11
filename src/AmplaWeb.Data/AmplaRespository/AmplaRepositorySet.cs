using DataWebServiceClient = AmplaWeb.Data.AmplaData2008.DataWebServiceClient;

namespace AmplaWeb.Data.AmplaRespository
{
    public class AmplaRepositorySet : IRepositorySet
    {
        public AmplaRepositorySet(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }
        
        private readonly string userName;
        private readonly string password;

        public IRepository<TModel> GetRepository<TModel>() where TModel : class, new()
        {
            DataWebServiceClient webServiceClient = new DataWebServiceClient("NetTcpBinding_IDataWebService");
            return new AmplaRepository<TModel>(webServiceClient, userName, password);
        }

        public IReadOnlyRepository<TModel> GetReadOnlyRepository<TModel>() where TModel : class, new()
        {
            DataWebServiceClient webServiceClient = new DataWebServiceClient("NetTcpBinding_IDataWebService");
            return new AmplaReadOnlyRepository<TModel>(webServiceClient, userName, password);
        }
    }
}
