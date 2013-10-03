using AmplaWeb.Data.Logging;

namespace AmplaWeb.Data.AmplaData2008
{
    public class LoggingDataWebServiceClient : IDataWebServiceClient
    {
        private readonly IDataWebServiceClient implementation;
        private readonly ILogger logger;

        public LoggingDataWebServiceClient(IDataWebServiceClient implementation, ILogger logger)
        {
            this.implementation = implementation;
            this.logger = logger;
        }

        public GetDataResponse GetData(GetDataRequest request)
        {
            logger.Log("GetData ({0})", request);
            return implementation.GetData(request);
        }

        public GetNavigationHierarchyResponse GetNavigationHierarchy(GetNavigationHierarchyRequest request)
        {
            logger.Log("GetNavigationHierarchy ({0})", request);
            return implementation.GetNavigationHierarchy(request);
        }

        public SubmitDataResponse SubmitData(SubmitDataRequest request)
        {
            logger.Log("SubmitData ({0})", request);
            return implementation.SubmitData(request);
        }

        public DeleteRecordsResponse DeleteRecords(DeleteRecordsRequest request)
        {
            logger.Log("DeleteRecords ({0})", request);
            return implementation.DeleteRecords(request);
        }

        public UpdateRecordStatusResponse UpdateRecordStatus(UpdateRecordStatusRequest request)
        {
            logger.Log("UpdateRecordStatus ({0})", request);
            return implementation.UpdateRecordStatus(request);
        }

        public GetViewsResponse GetViews(GetViewsRequest request)
        {
            logger.Log("GetViews ({0})", request);
            return implementation.GetViews(request);
        }

        public SplitRecordsResponse SplitRecords(SplitRecordsRequest request)
        {
            logger.Log("SplitRecords ({0})", request);
            return implementation.SplitRecords(request);
        }
    }
}