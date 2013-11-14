using AmplaData.WebService;

namespace AmplaData.AmplaData2008
{
    public class DataWebServiceFactory : WebServiceFactory<IDataWebServiceClient>, IDataWebServiceClient
    {
        public GetDataResponse GetData(GetDataRequest request)
        {
            return Create().GetData(request);
        }

        public GetAuditDataResponse GetAuditData(GetAuditDataRequest request)
        {
            return Create().GetAuditData(request);
        }

        public GetNavigationHierarchyResponse GetNavigationHierarchy(GetNavigationHierarchyRequest request)
        {
            return Create().GetNavigationHierarchy(request);
        }

        public SubmitDataResponse SubmitData(SubmitDataRequest request)
        {
            return Create().SubmitData(request);
        }

        public DeleteRecordsResponse DeleteRecords(DeleteRecordsRequest request)
        {
            return Create().DeleteRecords(request);
        }

        public UpdateRecordStatusResponse UpdateRecordStatus(UpdateRecordStatusRequest request)
        {
            return Create().UpdateRecordStatus(request);
        }

        public GetViewsResponse GetViews(GetViewsRequest request)
        {
            return Create().GetViews(request);
        }

        public SplitRecordsResponse SplitRecords(SplitRecordsRequest request)
        {
            return Create().SplitRecords(request);
        }
    }
}