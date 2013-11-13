
using AmplaData.Data.AmplaData2008;

namespace AmplaData.Data.Binding.Mapping.Modules
{
    public class ProductionModuleMapping : StandardModuleMapping
    {
        public ProductionModuleMapping()
        {
            AddSpecialMapping("SampleDateTime", () => new DefaultValueFieldMapping("Sample Period", Iso8601UtcNow));
            AddRequiredMapping("SampleDateTime", () => new DefaultValueFieldMapping("Sample Period", Iso8601UtcNow));

            AddSupportedOperation(ViewAllowedOperations.AddRecord);
            AddSupportedOperation(ViewAllowedOperations.DeleteRecord);
            AddSupportedOperation(ViewAllowedOperations.ModifyRecord);

            AddSupportedOperation(ViewAllowedOperations.ConfirmRecord);
            AddSupportedOperation(ViewAllowedOperations.UnconfirmRecord);
        }
    }
}