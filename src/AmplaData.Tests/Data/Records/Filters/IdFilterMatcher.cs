namespace AmplaData.Records.Filters
{

    public class IdFilterMatcher : FilterMatcher
    {
        private readonly int id;
        private readonly string stringId;

        public IdFilterMatcher(string id)
        {
            this.id = int.Parse(id);
            stringId = id;
        }

        public override bool Matches(InMemoryRecord record)
        {
            return record.RecordId == id;
        }

        public override bool Matches(InMemoryAuditRecord auditRecord)
        {
            return auditRecord.SetId == stringId;
        }
    }
}