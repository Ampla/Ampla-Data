using System;

namespace AmplaData.Data.Records.Filters
{
    public class SamplePeriodFilterMatcher : FieldFilterMatcher<DateTime>
    {
        private readonly string dateTime;

        public SamplePeriodFilterMatcher(string field, string value) : base(field, value)
        {
            dateTime = value;
        }

        public override bool Matches(InMemoryAuditRecord auditRecord)
        {
            return auditRecord.EditedDateTime == dateTime;
        }
    }
}