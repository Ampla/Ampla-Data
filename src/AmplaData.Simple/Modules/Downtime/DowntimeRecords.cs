﻿using System;
using AmplaData.Records;

namespace AmplaData.Modules.Downtime
{
    public static class DowntimeRecords
    {
        private static int _recordId = 100;

        public static InMemoryRecord NewRecord()
        {
            InMemoryRecord record = new InMemoryRecord(DowntimeViews.StandardView())
                {
                    Location = "Enterprise.Site.Area.Downtime",
                    Module = "Downtime"
                };
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