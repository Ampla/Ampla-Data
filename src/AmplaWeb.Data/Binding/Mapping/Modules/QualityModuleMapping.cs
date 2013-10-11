
namespace AmplaWeb.Data.Binding.Mapping.Modules
{
    public class QualityModuleMapping : StandardModuleMapping
    {
        public QualityModuleMapping()
        {
            AddSpecialMapping("SampleDateTime", () => new DefaultValueFieldMapping("Sample Period", Iso8601UtcNow));
            AddRequiredMapping("SampleDateTime", () => new DefaultValueFieldMapping("Sample Period", Iso8601UtcNow));
        }
    }
}