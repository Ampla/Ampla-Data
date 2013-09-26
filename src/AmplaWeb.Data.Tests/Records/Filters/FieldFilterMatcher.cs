namespace AmplaWeb.Data.Records.Filters
{
    public class FieldFilterMatcher<T> : FilterMatcher
    {
        private readonly string field;
        private readonly T value;

        public FieldFilterMatcher(string field, string value)
        {
            this.field = field;
            this.value = PersistenceHelper.ConvertFromString<T>(value);
        }

        public override bool Matches(InMemoryRecord record)
        {
            T fieldValue = record.GetFieldValue(field, default(T));

            return Equals(fieldValue, value);
        }
    }
}