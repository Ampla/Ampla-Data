
namespace AmplaWeb.Data.Binding.Mapping.Modules
{
    public class ProductionModuleMapping : StandardModuleMapping
    {
        public ProductionModuleMapping()
        {
            AddSpecialMapping("SampleDateTime", () => new DefaultValueFieldMapping("Sample Period", Iso8601UtcNow));
            AddRequiredMapping("SampleDateTime", () => new DefaultValueFieldMapping("Sample Period", Iso8601UtcNow));
        }
    }
}