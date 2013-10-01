namespace AmplaWeb.Data.Records.Filters
{
    public class LocationFilterMatcher : FilterMatcher
    {
        private readonly string location;

        public LocationFilterMatcher(string location)
        {
            this.location = location;
        }

        public override bool Matches(InMemoryRecord record)
        {
            return record.Location == location;
        }
    }
}