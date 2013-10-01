namespace AmplaWeb.Data.Records.Filters
{
    public abstract class FilterMatcher
    {
        public abstract bool Matches(InMemoryRecord record);
    }
}