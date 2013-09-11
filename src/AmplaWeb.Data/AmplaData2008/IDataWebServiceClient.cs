
namespace AmplaWeb.Data.AmplaData2008
{
    public interface IDataWebServiceClient
    {
        GetDataResponse GetData(GetDataRequest request);
        GetNavigationHierarchyResponse GetNavigationHierarchy(GetNavigationHierarchyRequest request);
        SubmitDataResponse SubmitData(SubmitDataRequest request);
        DeleteRecordsResponse DeleteRecords(DeleteRecordsRequest request);
        UpdateRecordStatusResponse UpdateRecordStatus(UpdateRecordStatusRequest request);
        GetViewsResponse GetViews(GetViewsRequest request);
        SplitRecordsResponse SplitRecords(SplitRecordsRequest request);
    }
}