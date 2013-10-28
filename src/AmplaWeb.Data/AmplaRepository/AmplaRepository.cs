using System;
using System.Collections.Generic;
using System.Linq;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding;
using AmplaWeb.Data.Binding.ModelData;
using AmplaWeb.Data.Binding.ViewData;
using AmplaWeb.Data.Records;

namespace AmplaWeb.Data.AmplaRepository
{
    /// <summary>
    ///     Ampla Repository that allows the manipulation of Ampla models
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class AmplaRepository<TModel> : IRepository<TModel> where TModel : class, new()
    {

        private readonly Dictionary<string, IAmplaViewProperties> amplaViewDictionary;
        private IDataWebServiceClient webServiceClient;
        private readonly ICredentialsProvider credentialsProvider;
        private readonly IModelProperties<TModel> modelProperties;

        public AmplaRepository(IDataWebServiceClient webServiceClient, ICredentialsProvider credentialsProvider)
        {
            this.webServiceClient = webServiceClient;
            this.credentialsProvider = credentialsProvider;
            modelProperties = new ModelProperties<TModel>();
            amplaViewDictionary = new Dictionary<string, IAmplaViewProperties>();
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
            IAmplaViewProperties amplaViewProperties = GetViewProperties(null);
            amplaViewProperties.Enforce.CanView();

            var request = GetDataRequest(true);
            GetDataResponse response = webServiceClient.GetData(request);

            List<TModel> records = new List<TModel>();
            IAmplaBinding binding = new AmplaGetDataBinding<TModel>(response, records, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                return records;
            }

            return null;
        }
        
        /// <summary>
        /// Finds the model using the id.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns></returns>
        public TModel FindById(int id)
        {
            IAmplaViewProperties amplaViewProperties = GetViewProperties(null);
            amplaViewProperties.Enforce.CanView();

            FilterValue filter = new FilterValue("Id", Convert.ToString(id));
            var request = GetDataRequest(false, filter);
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
        /// Finds the record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AmplaRecord FindRecord(int id)
        {
            IAmplaViewProperties amplaViewProperties = GetViewProperties(null);
            amplaViewProperties.Enforce.CanView();

            FilterValue idFilter = new FilterValue("Id", Convert.ToString(id));
            FilterValue deletedFilter = new FilterValue("Deleted", "");
            var request = GetDataRequest(false, idFilter, deletedFilter);
            GetDataResponse response = webServiceClient.GetData(request);
            TModel model;
            return FindAmplaRecord(response, ModelProperties, amplaViewProperties, out model);
        }

        private AmplaRecord FindAmplaRecord(GetDataResponse response, IModelProperties<TModel> iModelProperties,  IAmplaViewProperties amplaViewProperties, out TModel model)
        {
            List<AmplaRecord> records = new List<AmplaRecord>();
            model = null;
            IAmplaBinding amplaBinding = new AmplaGetDataRecordBinding<TModel>(response, records, modelProperties,
                                                                          amplaViewProperties);
            if (amplaBinding.Validate() && amplaBinding.Bind())
            {
                List<TModel> models = new List<TModel>();
                IAmplaBinding modelBinding = new AmplaGetDataBinding<TModel>(response, models, iModelProperties);
                if (modelBinding.Validate() && modelBinding.Bind())
                {
                    model = models.Count > 0 ? models[0] : null;
                }
                return records.Count > 0 ? records[0] : null;
            }
            return null;
        }

        /// <summary>
        /// Gets the history for the record
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns></returns>
        public AmplaAuditRecord GetHistory(int id)
        {
            IAmplaViewProperties amplaViewProperties = GetViewProperties(null);
            amplaViewProperties.Enforce.CanView();

            FilterValue idFilter = new FilterValue("Id", Convert.ToString(id));
            FilterValue deletedFilter = new FilterValue("Deleted", "");
            GetDataRequest dataRequest = GetDataRequest(false, idFilter, deletedFilter);
            GetDataResponse dataResponse = webServiceClient.GetData(dataRequest);
            TModel model;
            AmplaRecord amplaRecord = FindAmplaRecord(dataResponse, ModelProperties, amplaViewProperties, out model);

            if (amplaRecord != null)
            {
                var request = GetAuditDataRequest(amplaRecord);
                GetAuditDataResponse response = webServiceClient.GetAuditData(request);
                List<AmplaAuditRecord> auditRecords = new List<AmplaAuditRecord>();
                IAmplaBinding binding = new AmplaGetAuditDataRecordBinding<TModel>(response, amplaRecord, auditRecords, modelProperties);
                if (binding.Validate() && binding.Bind())
                {
                    return auditRecords.Count > 0 ? auditRecords[0] : null;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the versions of the model
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns></returns>
        public ModelVersions GetVersions(int id)
        {
            IAmplaViewProperties amplaViewProperties = GetViewProperties(null);
            amplaViewProperties.Enforce.CanView();

            FilterValue idFilter = new FilterValue("Id", Convert.ToString(id));
            FilterValue deletedFilter = new FilterValue("Deleted", "");
            GetDataRequest dataRequest = GetDataRequest(false, idFilter, deletedFilter);
            GetDataResponse dataResponse = webServiceClient.GetData(dataRequest);
            TModel model;
            AmplaRecord amplaRecord = FindAmplaRecord(dataResponse, ModelProperties, amplaViewProperties, out model);

            if (amplaRecord != null)
            {
                var request = GetAuditDataRequest(amplaRecord);
                GetAuditDataResponse response = webServiceClient.GetAuditData(request);
                List<AmplaAuditRecord> auditRecords = new List<AmplaAuditRecord>();
                IAmplaBinding binding = new AmplaGetAuditDataRecordBinding<TModel>(response, amplaRecord, auditRecords, modelProperties);
                if (binding.Validate() && binding.Bind())
                {
                    AmplaAuditRecord auditRecord = auditRecords.Count > 0 ? auditRecords[0] : null;
                    ModelVersions modelVersions = new ModelVersions(amplaRecord);
                    IAmplaBinding historyBinding = new AmplaGetDataVersionsBinding<TModel>(amplaRecord, auditRecord, model, modelVersions, modelProperties, amplaViewProperties);
                    if (historyBinding.Validate() && historyBinding.Bind())
                    {
                        return modelVersions;
                    }
                }
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
            IAmplaViewProperties amplaViewProperties = GetViewProperties(null);
            amplaViewProperties.Enforce.CanView();
            
            var request = GetDataRequest(true, filters);
            GetDataResponse response = webServiceClient.GetData(request);

            List<TModel> records = new List<TModel>();
            IAmplaBinding binding = new AmplaGetDataBinding<TModel>(response, records, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                return records;
            }

            return null;
        }
        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Add(TModel model)
        {   
            IAmplaViewProperties amplaViewProperties = GetViewProperties(model);
            amplaViewProperties.Enforce.CanAdd();

            SubmitDataRequest request = new SubmitDataRequest {Credentials = CreateCredentials()};
            List<SubmitDataRecord> records = new List<SubmitDataRecord>();
            List<TModel> models = new List<TModel> { model };

            IAmplaBinding binding = new AmplaAddDataBinding<TModel>(models, records, amplaViewProperties, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                request.SubmitDataRecords = records.ToArray();
                SubmitDataResponse response = WebServiceClient.SubmitData(request);
                
                IAmplaBinding resultBinding = new AmplaDataSubmissionResultBinding<TModel>(response.DataSubmissionResults, models, ModelProperties);
                if (resultBinding.Validate())
                {
                    resultBinding.Bind();
                }
            }
        }

        /// <summary>
        /// Deletes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Delete(TModel model)
        {
            IAmplaViewProperties amplaViewProperties = GetViewProperties(model);
            amplaViewProperties.Enforce.CanDelete();

            DeleteRecordsRequest request = new DeleteRecordsRequest {Credentials = CreateCredentials()};
            List<DeleteRecord> records = new List<DeleteRecord>();
            List<TModel> models = new List<TModel>{model};
            
            IAmplaBinding binding = new AmplaDeleteDataBinding<TModel>(models, records, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                request.DeleteRecords = records.ToArray();
                WebServiceClient.DeleteRecords(request);
            }
        }

        /// <summary>
        /// Confirms the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Confirm(TModel model)
        {
            IAmplaViewProperties amplaViewProperties = GetViewProperties(model);
            amplaViewProperties.Enforce.CanConfirm();

            UpdateRecordStatusRequest request = new UpdateRecordStatusRequest {Credentials = CreateCredentials()};
            List<UpdateRecordStatus> records = new List<UpdateRecordStatus>();
            List<TModel> models = new List<TModel> { model };

            IAmplaBinding binding = new AmplaConfirmDataBinding<TModel>(models, records, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                request.UpdateRecords = records.ToArray();
                WebServiceClient.UpdateRecordStatus(request);
            }
        }

        /// <summary>
        /// Unconfirms the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Unconfirm(TModel model)
        {
            IAmplaViewProperties amplaViewProperties = GetViewProperties(model);
            amplaViewProperties.Enforce.CanUnconfirm();

            UpdateRecordStatusRequest request = new UpdateRecordStatusRequest {Credentials = CreateCredentials()};
            List<UpdateRecordStatus> records = new List<UpdateRecordStatus>();
            List<TModel> models = new List<TModel> { model };

            IAmplaBinding binding = new AmplaUnconfirmDataBinding<TModel>(models, records, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                request.UpdateRecords = records.ToArray();
                WebServiceClient.UpdateRecordStatus(request);
            }
        }

        /// <summary>
        /// Gets the allowed values.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public List<string> GetAllowedValues(string property)
        {
            if (property == "Location")
            {
                GetNavigationHierarchyRequest request = new GetNavigationHierarchyRequest
                    {
                        Credentials = CreateCredentials(),
                        Module = ModelProperties.Module,
                        Context = NavigationContext.Plant,
                        Mode = NavigationMode.Location
                    };

                GetNavigationHierarchyResponse response = WebServiceClient.GetNavigationHierarchy(request);
                List<string> values = new List<string>();
                IAmplaBinding binding = new AmplaNavigationBinding(response, values);
                if (binding.Validate() && binding.Bind())
                {
                    return values;
                }
            }

            return null;
        }

        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Update(TModel model)
        {
            IAmplaViewProperties amplaViewProperties = GetViewProperties(model);
            amplaViewProperties.Enforce.CanModify();

            List<int> identifiers = new List<int>();

            TModel existing = null;
            IAmplaBinding modelBinding = new ModelIdentifierBinding<TModel>(model, identifiers, ModelProperties);
            if (modelBinding.Validate() && modelBinding.Bind())
            {
                existing = FindById(identifiers[0]);
            }
            
            SubmitDataRequest request = new SubmitDataRequest {Credentials = CreateCredentials()};
            List<SubmitDataRecord> records = new List<SubmitDataRecord>();

            IAmplaBinding binding = new AmplaUpdateDataBinding<TModel>(existing, model, records, amplaViewProperties, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                request.SubmitDataRecords = records.ToArray();
                WebServiceClient.SubmitData(request);
            }
        }

        private GetDataRequest GetDataRequest(bool includeDefaultFilters, params FilterValue[] filters)
        {
            GetDataRequest request = new GetDataRequest
            {
                Credentials = CreateCredentials(),
                Metadata = true,
                OutputOptions = new GetDataOutputOptions
                {
                    ResolveIdentifiers = ModelProperties.ResolveIdentifiers,
                },
                Filter = GetDataFilter(includeDefaultFilters, filters),
                View = new GetDataView
                {
                    Context = NavigationContext.Plant,
                    Mode = NavigationMode.Location,
                    Module = ModelProperties.Module
                }
            };
            return request;
        }

        public GetAuditDataRequest GetAuditDataRequest(AmplaRecord record)
        {
            GetAuditDataRequest request = new GetAuditDataRequest
                {
                    Credentials = CreateCredentials(),
                    Filter = new GetAuditDataFilter
                        {
                            Location = record.Location,
                            Module = ModelProperties.Module,
                            SetId = Convert.ToString(record.Id)
                        }
                };
            return request;
        }

        private DataFilter GetDataFilter(bool includeDefaultFilters, IEnumerable<FilterValue> filters)
        {
            DataFilter dataFilter = new DataFilter {Location = ModelProperties.LocationFilter.Filter};

            Dictionary<string, FilterEntry> filterDictionary = new Dictionary<string, FilterEntry>();

            List<FilterValue> mergedFilters = new List<FilterValue>();
            if (includeDefaultFilters)
            {
                mergedFilters.AddRange(ModelProperties.DefaultFilters);
            }
            mergedFilters.AddRange(filters ?? new FilterValue[0]);

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
            return credentialsProvider.GetCredentials();
        }


        /// <summary>
        /// Gets the view properties.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected IAmplaViewProperties GetViewProperties(TModel model)
        {
            IAmplaViewProperties amplaView;

            string location;
            bool checkForValidLocation;

            if (model == null)
            {
                location = ModelProperties.LocationFilter.Location;
                checkForValidLocation = false;
            }
            else
            {
                location = ModelProperties.GetLocation(model) ?? "";
                checkForValidLocation = true;
            }
            
            if (!amplaViewDictionary.TryGetValue(location, out amplaView))
            {
                if (checkForValidLocation)
                {
                    CheckLocationIsValid(location);
                }

                AmplaViewProperties<TModel> viewProperties = new AmplaViewProperties<TModel>(ModelProperties);
                GetViewsRequest request = new GetViewsRequest
                    {
                        Credentials = CreateCredentials(),
                        Mode = NavigationMode.Location,
                        Context = NavigationContext.Plant,
                        ViewPoint = location,
                        Module = ModelProperties.Module,
                        Localization = new Localization()
                    };

                GetViewsResponse response = WebServiceClient.GetViews(request);
                viewProperties.Initialise(response);
                amplaViewDictionary[location] = viewProperties;
                amplaView = viewProperties;
            }
            return amplaView;
        }

        /// <summary>
        /// Checks the location is valid.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        protected void CheckLocationIsValid(string location)
        {
            List<string> locations = GetAllowedValues("Location");
            if (!locations.Contains(location))
            {
                string message = string.Format("Location '{0}' is not valid. Available Locations are: \r\n - {1}", location,
                                               string.Join("\r\n - ", locations));
                throw new InvalidOperationException(message);
            }
        }
    }
}