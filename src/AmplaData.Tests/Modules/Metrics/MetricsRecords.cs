using System;
using AmplaData.Records;

namespace AmplaData.Modules.Metrics
{
    public static class MetricsRecords
    {
        private static int _recordId = 100;

        public static InMemoryRecord NewRecord()
        {
            InMemoryRecord record = new InMemoryRecord { Location = "Enterprise.Site.Area.Metrics", Module = "Metrics" };
            record.SetFieldValue("IsManual", false);
            record.SetFieldValue("Deleted", false);
            record.SetFieldValue("Start Time", DateTime.Today);
            record.SetFieldValue("End Time", DateTime.Today.AddHours(1));
            record.SetFieldValue("Period", "Hour");
            record.SetFieldValue("Duration", 3600);
            record.RecordId = _recordId++;
            return record;
        }
    }
}