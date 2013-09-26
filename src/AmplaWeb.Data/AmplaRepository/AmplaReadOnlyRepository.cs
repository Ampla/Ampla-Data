using System;
using System.Collections.Generic;
using System.Linq;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding;
using AmplaWeb.Data.Binding.ModelData;
using AmplaWeb.Data.Binding.ViewData;

namespace AmplaWeb.Data.AmplaRepository
{
    public class AmplaReadOnlyRepository<TModel> : IReadOnlyRepository<TModel> where TModel : class, new()
    {
        private IDataWebServiceClient webServiceClient;
        private readonly string userName;
        private readonly string password;
        private readonly IModelProperties<TModel> modelProperties; 

        public AmplaReadOnlyRepository(IDataWebServiceClient webServiceClient, string userName, string password)
        {
            this.webServiceClient = webServiceClient;
            this.userName = userName;
            this.password = password;
            modelProperties = new ModelProperties<TModel>();
         }

        public void Dispose()
        {
            webServiceClient = null;
        }

        protected IModelProperties<TModel> ModelProperties
        {
            get { return modelProperties; }
        }



        protected IDataWebServiceClient WebServiceClient
        {
            get { return webServiceClient; }
        }

        public IList<TModel> GetAll()
        {
            var request = GetDataRequest();
            GetDataResponse response = webServiceClient.GetData(request);

            List<TModel> records = new List<TModel>();
            IAmplaBinding binding = new AmplaGetDataBinding<TModel>(response, records, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                return records;
            }

            return null;
        }

        private GetDataRequest GetDataRequest(params FilterValue[] filters)
        {
            GetDataRequest request = new GetDataRequest
                {
                    Credentials = CreateCredentials(),
                    Metadata = true,
                    OutputOptions = new GetDataOutputOptions
                        {
                            ResolveIdentifiers = true
                        },
                    Filter = new DataFilter
                        {
                            Location = ModelProperties.LocationFilter.Filter,
                            Criteria = GetFilters(filters)
                        },
                    View = new GetDataView
                        {
                            Context = NavigationContext.Plant,
                            Mode = NavigationMode.Location,
                            Module = ModelProperties.Module
                        }
                };
            return request;
        }

        private static FilterEntry[] GetFilters(IEnumerable<FilterValue> filters)
        {
            if (filters == null)
            {
                return null;
            }

            return filters.Select(filter => new FilterEntry {Name = filter.Name, Value = filter.Value}).ToArray();
        }

        protected Credentials CreateCredentials()
        {
            return new Credentials {Username = userName, Password = password};
        }

        public TModel FindById(int id)
        {
            FilterValue filter = new FilterValue("Id", Convert.ToString(id));
            var request = GetDataRequest(filter);
            GetDataResponse response = webServiceClient.GetData(request);

            List<TModel> records = new List<TModel>();
            IAmplaBinding binding = new AmplaGetDataBinding<TModel>(response, records, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                return records.Count == 1 ? records[0] : null;    
            }

            return null;
        }

        public IList<TModel> FindByFilter(params FilterValue[] filters)
        {
            var request = GetDataRequest(filters);
            GetDataResponse response = webServiceClient.GetData(request);

            List<TModel> records = new List<TModel>();
            IAmplaBinding binding = new AmplaGetDataBinding<TModel>(response, records, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                return records;
            }

            return null;
        }
    }
}