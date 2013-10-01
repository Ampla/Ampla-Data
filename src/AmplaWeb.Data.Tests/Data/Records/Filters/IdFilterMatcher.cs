namespace AmplaWeb.Data.Records.Filters
{

    public class IdFilterMatcher : FilterMatcher
    {
        private readonly int id;

        public IdFilterMatcher(string id)
        {
            this.id = int.Parse(id);
        }

        public override bool Matches(InMemoryRecord record)
        {
            return record.RecordId == id;
        }
    }
}