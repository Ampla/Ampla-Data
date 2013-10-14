
using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Binding.Mapping.Modules
{
    public class QualityModuleMapping : StandardModuleMapping
    {
        public QualityModuleMapping()
        {
            AddSpecialMapping("SampleDateTime", () => new DefaultValueFieldMapping("Sample Period", Iso8601UtcNow));
            AddRequiredMapping("SampleDateTime", () => new DefaultValueFieldMapping("Sample Period", Iso8601UtcNow));

            AddAllowedOperation(ViewAllowedOperations.AddRecord);
            AddAllowedOperation(ViewAllowedOperations.DeleteRecord);
            AddAllowedOperation(ViewAllowedOperations.ModifyRecord);

            AddAllowedOperation(ViewAllowedOperations.ConfirmRecord);
            AddAllowedOperation(ViewAllowedOperations.UnconfirmRecord);
        }
    }
}