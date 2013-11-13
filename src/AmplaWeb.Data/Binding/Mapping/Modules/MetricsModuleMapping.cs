namespace AmplaData.Data.Binding.Mapping.Modules
{
    public class MetricsModuleMapping: StandardModuleMapping
    {
        public MetricsModuleMapping()
        {
            // Add Special Field Mappings for Metrics fields
            //AddSpecialMapping("StartDateTime", () => new DefaultValueFieldMapping("Start Time", Iso8601UtcNow));
            //AddSpecialMapping("EndDateTime", () => new DefaultValueFieldMapping("End Time", Iso8601UtcNow));

            // Add a required field mapping for Metrics fields
        }
    }
}