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
        private readonly Dictionary<string, IAmplaViewProperties<TModel>> amplaViewDictionary; 
        private readonly IModelProperties<TModel> modelProperties; 

        public AmplaReadOnlyRepository(IDataWebServiceClient webServiceClient, string userName, string password)
        {
            this.webServiceClient = webServiceClient;
            this.userName = userName;
            this.password = password;
            modelProperties = new ModelProperties<TModel>();
            amplaViewDictionary = new Dictionary<string, IAmplaViewProperties<TModel>>();
        }

        public void Dispose()
        {
            webServiceClient = null;
        }

        protected IModelProperties<TModel> ModelProperties
        {
            get { return modelProperties; }
        }

        protected IAmplaViewProperties<TModel> GetViewProperties(TModel model)
        {
            IAmplaViewProperties<TModel> amplaView;
            string location = ModelProperties.GetLocation(model);
            if (!amplaViewDictionary.TryGetValue(location, out amplaView))
            {
                AmplaViewProperties<TModel> viewProperties = new AmplaViewProperties<TModel>(ModelProperties);
                GetViewsRequest request = new GetViewsRequest();
                request.Credentials = CreateCredentials();
                request.Mode = NavigationMode.Location;
                request.Context = NavigationContext.Plant;
                request.ViewPoint = location;
                request.Module = modelProperties.Module;

                GetViewsResponse response = WebServiceClient.GetViews(request);
                viewProperties.Initialise(response);
                amplaViewDictionary[location] = viewProperties;
                amplaView = viewProperties;
            }
            return amplaView;
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
            if (new AmplaGetDataBinding<TModel>(response, records, ModelProperties).Bind())
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
                            Location = ModelProperties.FilterLocation,
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
            if (new AmplaGetDataBinding<TModel>(response, records, ModelProperties).Bind())
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
            if (new AmplaGetDataBinding<TModel>(response, records, ModelProperties).Bind())
            {
                return records;
            }

            return null;
        }
    }
}