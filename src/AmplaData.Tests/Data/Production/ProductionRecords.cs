using System;
using AmplaData.Data.Records;

namespace AmplaData.Data.Production
{
    public static class ProductionRecords
    {
        private static int _recordId = 100;

        public static InMemoryRecord NewRecord()
        {
            InMemoryRecord record = new InMemoryRecord {Location = "Enterprise.Site.Area.Production", Module = "Production"};
            record.SetFieldValue("IsManual", false);
            record.SetFieldValue("Deleted", false);
            record.SetFieldValue("Confirmed", false);
            record.SetFieldValue("Sample Period", DateTime.Now.TrimToSeconds());
            record.SetFieldValue("Duration", 90);
            record.SetFieldValue("Field 1", 100);
            record.SetFieldValue("Unique", Guid.NewGuid());
            record.RecordId = _recordId++;
            return record;
        }
    }
}