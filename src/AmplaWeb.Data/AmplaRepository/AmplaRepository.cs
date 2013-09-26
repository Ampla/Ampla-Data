using System;
using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding;
using AmplaWeb.Data.Binding.ViewData;

namespace AmplaWeb.Data.AmplaRepository
{
    public class AmplaRepository<TModel> : AmplaReadOnlyRepository<TModel>, IRepository<TModel> where TModel : class, new()
    {

        private readonly Dictionary<string, IAmplaViewProperties<TModel>> amplaViewDictionary; 

        public AmplaRepository(IDataWebServiceClient webServiceClient, string userName, string password) : base(webServiceClient, userName, password)
        {
            amplaViewDictionary = new Dictionary<string, IAmplaViewProperties<TModel>>();
        }

        public void Add(TModel model)
        {
            SubmitDataRequest request = new SubmitDataRequest();
            request.Credentials = CreateCredentials();
            List<SubmitDataRecord> records = new List<SubmitDataRecord>();
            List<TModel> models = new List<TModel> { model };

            IAmplaViewProperties<TModel> amplaViewProperties = GetViewProperties(model);

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

        public void Delete(TModel model)
        {
            DeleteRecordsRequest request = new DeleteRecordsRequest();
            request.Credentials = CreateCredentials();
            List<DeleteRecord> records = new List<DeleteRecord>();
            List<TModel> models = new List<TModel>{model};
            
            IAmplaBinding binding = new AmplaDeleteDataBinding<TModel>(models, records, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                request.DeleteRecords = records.ToArray();
                WebServiceClient.DeleteRecords(request);
            }
        }

        public void Confirm(TModel model)
        {
            UpdateRecordStatusRequest request = new UpdateRecordStatusRequest();
            request.Credentials = CreateCredentials();
            List<UpdateRecordStatus> records = new List<UpdateRecordStatus>();
            List<TModel> models = new List<TModel> { model };

            IAmplaBinding binding = new AmplaConfirmDataBinding<TModel>(models, records, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                request.UpdateRecords = records.ToArray();
                WebServiceClient.UpdateRecordStatus(request);
            }
        }

        public void Unconfirm(TModel model)
        {
            UpdateRecordStatusRequest request = new UpdateRecordStatusRequest();
            request.Credentials = CreateCredentials();
            List<UpdateRecordStatus> records = new List<UpdateRecordStatus>();
            List<TModel> models = new List<TModel> { model };

            IAmplaBinding binding = new AmplaUnconfirmDataBinding<TModel>(models, records, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                request.UpdateRecords = records.ToArray();
                WebServiceClient.UpdateRecordStatus(request);
            }
        }

        public List<string> GetAllowedValues(string property)
        {
            if (property == "Location")
            {
                GetNavigationHierarchyRequest request = new GetNavigationHierarchyRequest();
                request.Credentials = CreateCredentials();
                request.Module = ModelProperties.Module;
                request.Context = NavigationContext.Plant;
                request.Mode = NavigationMode.Location;

                GetNavigationHierarchyResponse response = WebServiceClient.GetNavigationHierarchy(request);
                List<string> values = new List<string>();
                IAmplaBinding binding = new AmplaNavigationBinding<TModel>(response, values, ModelProperties);
                if (binding.Validate() && binding.Bind())
                {
                    return values;
                }
            }

            return null;
        }

        public void Update(TModel model)
        {
            List<int> identifiers = new List<int>();

            TModel existing = null;
            IAmplaBinding modelBinding = new ModelIdentifierBinding<TModel>(model, identifiers, ModelProperties);
            if (modelBinding.Validate() && modelBinding.Bind())
            {
                existing = FindById(identifiers[0]);
            }
            
            SubmitDataRequest request = new SubmitDataRequest();
            request.Credentials = CreateCredentials();
            List<SubmitDataRecord> records = new List<SubmitDataRecord>();

            IAmplaViewProperties<TModel> amplaViewProperties = GetViewProperties(model);

            IAmplaBinding binding = new AmplaUpdateDataBinding<TModel>(existing, model, records, amplaViewProperties, ModelProperties);
            if (binding.Validate() && binding.Bind())
            {
                request.SubmitDataRecords = records.ToArray();
                WebServiceClient.SubmitData(request);
            }
        }

        protected IAmplaViewProperties<TModel> GetViewProperties(TModel model)
        {
            IAmplaViewProperties<TModel> amplaView;
            string location = ModelProperties.GetLocation(model) ?? "";
        
            if (!amplaViewDictionary.TryGetValue(location, out amplaView))
            {
                CheckLocationIsValid(location);

                AmplaViewProperties<TModel> viewProperties = new AmplaViewProperties<TModel>(ModelProperties);
                GetViewsRequest request = new GetViewsRequest();
                request.Credentials = CreateCredentials();
                request.Mode = NavigationMode.Location;
                request.Context = NavigationContext.Plant;
                request.ViewPoint = location;
                request.Module = ModelProperties.Module;

                GetViewsResponse response = WebServiceClient.GetViews(request);
                viewProperties.Initialise(response);
                amplaViewDictionary[location] = viewProperties;
                amplaView = viewProperties;
            }
            return amplaView;
        }

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