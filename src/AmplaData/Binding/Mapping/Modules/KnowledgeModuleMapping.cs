
using System;
using AmplaData.AmplaData2008;

namespace AmplaData.Binding.Mapping.Modules
{
    public class KnowledgeModuleMapping : StandardModuleMapping
    {
        public KnowledgeModuleMapping()
        {
            AddSpecialMapping("SampleDateTime", () => new DefaultValueFieldMapping<DateTime>("Sample Period", Iso8601UtcNow));
            AddRequiredMapping("SampleDateTime", () => new RequiredFieldMapping<DateTime>("Sample Period", Iso8601UtcNow));

            AddSupportedOperation(ViewAllowedOperations.AddRecord);
            AddSupportedOperation(ViewAllowedOperations.DeleteRecord);
            AddSupportedOperation(ViewAllowedOperations.ModifyRecord);

            AddSupportedOperation(ViewAllowedOperations.ConfirmRecord);
            AddSupportedOperation(ViewAllowedOperations.UnconfirmRecord);
        }
    }
}