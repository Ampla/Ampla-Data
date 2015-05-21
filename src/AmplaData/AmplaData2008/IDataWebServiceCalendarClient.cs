namespace AmplaData.AmplaData2008
{
    public interface IDataWebServiceCalendarClient
    {
        DeleteCalendarValueResponse DeleteCalendarValue(DeleteCalendarValueRequest request);

        string[] GetCalendarNames(GetCalendarNamesRequest request);
        
        GetCalendarValuesResponse GetCalendarValues(GetCalendarValuesRequest request);

        SubmitCalendarValuesResponse SubmitCalendarValues(SubmitCalendarValuesRequest request);
    }
}