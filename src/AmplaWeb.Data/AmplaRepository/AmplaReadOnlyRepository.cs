using System;
using System.Collections.Generic;
using System.Linq;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding;
using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.AmplaRepository
{
    /// <summary>
    ///     The Ampla ReadOnly Repository allows the reading of Ampla models
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class AmplaReadOnlyRepository<TModel> : IReadOnlyRepository<TModel> where TModel : class, new()
    {
        private IDataWebServiceClient webServiceClient;
        private readonly string userName;
        private readonly string password;
        private readonly IModelProperties<TModel> modelProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaReadOnlyRepository{TModel}"/> class.
        /// </summary>
        /// <param name="webServiceClient">The web service client.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public AmplaReadOnlyRepository(IDataWebServiceClient webServiceClient, string userName, string password)
        {
            this.webServiceClient = webServiceClient;
            this.userName = userName;
            this.password = password;
            modelProperties = new ModelProperties<TModel>();
         }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            webServiceClient = null;
        }

        /// <summary>
        /// Gets the model properties.
        /// </summary>
        /// <value>
        /// The model properties.
        /// </value>
        protected IModelProperties<TModel> ModelProperties
        {
            get { return modelProperties; }
        }

        /// <summary>
        /// Gets the web service client.
        /// </summary>
        /// <value>
        /// The web service client.
        /// </value>
        protected IDataWebServiceClient WebServiceClient
        {
            get { return webServiceClient; }
        }

        /// <summary>
        /// Gets all the models 
        /// </summary>
        /// <returns></returns>
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
                    Filter = GetDataFilter(filters),
                    View = new GetDataView
                        {
                            Context = NavigationContext.Plant,
                            Mode = NavigationMode.Location,
                            Module = ModelProperties.Module
                        }
                };
            return request;
        }

        private DataFilter GetDataFilter(FilterValue[] filters)
        {
            DataFilter dataFilter = new DataFilter();
            dataFilter.Location = ModelProperties.LocationFilter.Filter;
            
            Dictionary<string, FilterEntry> filterDictionary = new Dictionary<string, FilterEntry>();

            List<FilterValue> mergedFilters = new List<FilterValue>();
            mergedFilters.AddRange(ModelProperties.DefaultFilters);
            mergedFilters.AddRange(filters);

            foreach (FilterValue filter in mergedFilters)
            {
                switch (filter.Name)
                {
                    case "Location":
                        {
                            // ignore Location
                            break;
                        }
                    case "Deleted":
                        {
                            dataFilter.Deleted = filter.Value;
                            break;
                        }
                    case "Sample Period":
                        {
                            dataFilter.SamplePeriod = filter.Value;
                            break;
                        }
                    default:
                        {
                            filterDictionary[filter.Name] = new FilterEntry { Name = filter.Name, Value = filter.Value }; 
                            break;
                        }
                }
                
            }
            dataFilter.Criteria = filterDictionary.Count == 0 ? null : filterDictionary.Values.ToArray();
            return dataFilter;
        }

        /// <summary>
        /// Creates the credentials.
        /// </summary>
        /// <returns></returns>
        protected Credentials CreateCredentials()
        {
            return new Credentials {Username = userName, Password = password};
        }

        /// <summary>
        /// Finds the model using the id.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Finds the models that match the filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
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