using System;
using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.Binding;
using AmplaData.Dynamic.Binding;
using AmplaData.Dynamic.Binding.ModelData;
using AmplaData.Dynamic.Binding.ViewData;

namespace AmplaData.Dynamic
{
    public class DynamicViewPointOperations : IDynamicViewPointOperations
    {
        private readonly DynamicViewPoint viewPoint;
        private readonly IDataWebServiceClient webServiceClient;
        private readonly ICredentialsProvider credentialsProvider;
        private readonly IDynamicModelProperties modelProperties;
        private readonly DynamicAmplaViewProperties viewProperties;

        public DynamicViewPointOperations(DynamicViewPoint viewPoint, IDataWebServiceClient webServiceClient, ICredentialsProvider credentialsProvider)
        {
            this.viewPoint = viewPoint;
            this.webServiceClient = webServiceClient;
            this.credentialsProvider = credentialsProvider;

            modelProperties = new DynamicModelProperties(viewPoint);

            viewProperties = new DynamicAmplaViewProperties(modelProperties);

            Initialise();
        }

        private void Initialise()
        {
            GetViewsRequest request = new GetViewsRequest
                {
                    Credentials = CreateCredentials(),
                    Module = modelProperties.Module,
                    Context = NavigationContext.Plant,
                    ViewPoint = viewPoint.Location
                };

            GetViewsResponse response = webServiceClient.GetViews(request);
            viewProperties.Initialise(response);
        }

        public DynamicViewPointOperations(DynamicViewPoint viewPoint) : this(viewPoint, DataWebServiceFactory.Create(), CredentialsProvider.ForUsernameAndPassword("User", "password"))
        {
        }

        public dynamic Save(object model)
        {
            IDynamicAmplaViewProperties amplaViewProperties = viewProperties;
            amplaViewProperties.Enforce.CanAdd();

            SubmitDataRequest request = new SubmitDataRequest {Credentials = CreateCredentials()};
            List<SubmitDataRecord> records = new List<SubmitDataRecord>();
            List<object> models = new List<object> {model.ToExpando()};

            IAmplaBinding binding = new AmplaAddDataDynamicBinding(models, records, amplaViewProperties, modelProperties);
            if (binding.Validate() && binding.Bind())
            {
                request.SubmitDataRecords = records.ToArray();
                SubmitDataResponse response = webServiceClient.SubmitData(request);

                IAmplaBinding resultBinding = new AmplaDataSubmissionResultDynamicBinding(response.DataSubmissionResults, models);
                if (resultBinding.Validate())
                {
                    resultBinding.Bind();
                }
            }
            return models[0];
        }

        private Credentials CreateCredentials()
        {
            return credentialsProvider.GetCredentials();
        }

        public dynamic Insert(object model)
        {
            throw new NotImplementedException();
        }

        public dynamic Update(object model)
        {
            throw new NotImplementedException();
        }
    }
}