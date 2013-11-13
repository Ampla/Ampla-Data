using System;
using AmplaData.Data.Records;

namespace AmplaData.Data.Energy
{
    public static class EnergyRecords
    {
        private static int _recordId = 100;

        public static InMemoryRecord NewRecord()
        {
            InMemoryRecord record = new InMemoryRecord { Location = "Enterprise.Site.Area.Energy", Module = "Energy" };
            record.SetFieldValue("IsManual", false);
            record.SetFieldValue("Deleted", false);
            record.SetFieldValue("Confirmed", false);
            record.SetFieldValue("Start Time", DateTime.Now.TrimToSeconds());
            record.SetFieldValue("Duration", 90);
            record.RecordId = _recordId++;
            return record;
        }
    }
}