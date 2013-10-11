
namespace AmplaWeb.Data.Binding.Mapping.Modules
{
    public class KnowledgeModuleMapping : StandardModuleMapping
    {
        public KnowledgeModuleMapping()
        {
            AddSpecialMapping("SampleDateTime", () => new DefaultValueFieldMapping("Sample Period", Iso8601UtcNow));
            AddRequiredMapping("SampleDateTime", () => new DefaultValueFieldMapping("Sample Period", Iso8601UtcNow));
        }
    }
}