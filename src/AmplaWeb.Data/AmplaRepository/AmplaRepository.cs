using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding;

namespace AmplaWeb.Data.AmplaRepository
{
    public class AmplaRepository<TModel> : AmplaReadOnlyRepository<TModel>, IRepository<TModel> where TModel : class, new()
    {
        public AmplaRepository(IDataWebServiceClient webServiceClient, string userName, string password) : base(webServiceClient, userName, password)
        {
        }

        public void Add(TModel model)
        {
            SubmitDataRequest request = new SubmitDataRequest();
            request.Credentials = CreateCredentials();
            List<SubmitDataRecord> records = new List<SubmitDataRecord>();
            List<TModel> models = new List<TModel> { model };
           
            if (new AmplaAddDataBinding<TModel>(models, records, ViewProperties, ModelProperties).Bind())
            {
                request.SubmitDataRecords = records.ToArray();
                WebServiceClient.SubmitData(request);
            }
        }

        public void Delete(TModel model)
        {
            DeleteRecordsRequest request = new DeleteRecordsRequest();
            request.Credentials = CreateCredentials();
            List<DeleteRecord> records = new List<DeleteRecord>();
            List<TModel> models = new List<TModel>{model};
            
            if (new AmplaDeleteDataBinding<TModel>(models, records, ModelProperties).Bind())
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
            if (new AmplaConfirmDataBinding<TModel>(models, records, ViewProperties).Bind())
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
            if (new AmplaUnconfirmDataBinding<TModel>(models, records, ViewProperties).Bind())
            {
                request.UpdateRecords = records.ToArray();
                WebServiceClient.UpdateRecordStatus(request);
            }
        }

        public void Update(TModel model)
        {
            SubmitDataRequest request = new SubmitDataRequest();
            request.Credentials = CreateCredentials();
            List<SubmitDataRecord> records = new List<SubmitDataRecord>();
            List<TModel> models = new List<TModel> { model };

            if (new AmplaUpdateDataBinding<TModel>(models, records, ViewProperties).Bind())
            {
                request.SubmitDataRecords = records.ToArray();
                WebServiceClient.SubmitData(request);
            }
        }
    }
}