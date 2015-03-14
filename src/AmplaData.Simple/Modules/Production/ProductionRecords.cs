using System;
using AmplaData.Records;

namespace AmplaData.Modules.Production
{
    public static class ProductionRecords
    {
        private static int _recordId = 100;

        public static InMemoryRecord NewRecord()
        {
            InMemoryRecord record = new InMemoryRecord(ProductionViews.StandardView())
                {
                    Location = "Enterprise.Site.Area.Production",
                    Module = "Production"
                };
            record.SetFieldValue("IsManual", false);
            record.SetFieldValue("Deleted", false);
            record.SetFieldValue("Confirmed", false);
            record.SetFieldValue("Sample Period", DateTime.Now.TrimToSeconds());
            record.SetFieldValue("Duration", 90);
            record.RecordId = _recordId++;
            return record;
        }
    }
}