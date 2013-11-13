namespace AmplaData.Data.Records.Filters
{
    public abstract class FilterMatcher
    {
        public abstract bool Matches(InMemoryRecord record);

        public abstract bool Matches(InMemoryAuditRecord auditRecord);
    }
}