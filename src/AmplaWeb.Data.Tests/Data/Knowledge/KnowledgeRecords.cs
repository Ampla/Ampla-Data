using System;
using AmplaWeb.Data.Records;

namespace AmplaWeb.Data.Knowledge
{
    public static class KnowledgeRecords
    {
        private static int _recordId = 100;

        public static InMemoryRecord NewRecord()
        {
            InMemoryRecord record = new InMemoryRecord { Location = "Enterprise.Site.Area.Knowledge", Module = "Knowledge" };
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