namespace AmplaData.Data.Records
{
    public class InMemoryAuditRecord
    {
        public string SetId { get; set; }
        public string Location { get; set; }
        public string EditedBy { get; set; }
        public string EditedDateTime { get; set; }
        public string Field { get; set; }
        public string OriginalValue { get; set; }
        public string EditedValue { get; set; }
        public string RecordType { get; set; }
    }
}