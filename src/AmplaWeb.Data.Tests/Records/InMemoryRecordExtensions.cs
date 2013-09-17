using System;
using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Records
{
    public static class InMemoryRecordExtensions
    {
        public static InMemoryRecord[] SplitRecord(this InMemoryRecord record, DateTime splitDateTimeUtc)
        {
            throw new NotImplementedException();
        }

        public static InMemoryRecord MarkAsNew(this InMemoryRecord record)
        {
            record.RecordId = -1;
            return record;
        }

        public static int SaveTo(this InMemoryRecord record, SimpleDataWebServiceClient webServiceClient)
        {
            SubmitDataRequest request = new SubmitDataRequest
                {
                    Credentials = webServiceClient.CreateCredentials(),
                    SubmitDataRecords = new[] {record.ConvertToSubmitDataRecord()}
                };
            SubmitDataResponse response = webServiceClient.SubmitData(request);

            int recordId = 0;
            if (response.DataSubmissionResults.Length > 0)
            {
                recordId = (int) response.DataSubmissionResults[0].SetId;
            }
            return recordId;
        }

    }
}