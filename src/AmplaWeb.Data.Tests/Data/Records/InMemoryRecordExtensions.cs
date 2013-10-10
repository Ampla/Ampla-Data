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
            return webServiceClient.AddExistingRecord(record);
        }

        public static bool IsDeleted(this InMemoryRecord record)
        {
            return record.GetFieldValue("Deleted", false);
        }


        public static bool IsConfirmed(this InMemoryRecord record)
        {
            return record.GetFieldValue("Confirmed", false);
        }

    }
}