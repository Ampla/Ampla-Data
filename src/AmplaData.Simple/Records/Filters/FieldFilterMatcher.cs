namespace AmplaData.Records.Filters
{
    public class FieldFilterMatcher<T> : FilterMatcher
    {
        private readonly string field;
        private readonly T value;
        private readonly string stringValue;

        public FieldFilterMatcher(string field, string value)
        {
            this.field = field;
            this.value = PersistenceHelper.ConvertFromString<T>(value);
            stringValue = value;
        }

        public override bool Matches(InMemoryRecord record)
        {
            T fieldValue = record.GetFieldValue(field, default(T));

            return Equals(fieldValue, value);
        }

        public override bool Matches(InMemoryAuditRecord auditRecord)
        {
            return auditRecord.Field == field && auditRecord.EditedValue == stringValue; 
        }
    }
}