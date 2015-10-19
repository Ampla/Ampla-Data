
using System;
using AmplaData.AmplaData2008;

namespace AmplaData.Binding.Mapping.Modules
{
    public class PlanningModuleMapping : StandardModuleMapping
    {
        public PlanningModuleMapping()
        {
            AddSpecialMapping("PlannedStartDateTime", () => new DefaultValueFieldMapping<DateTime>("Planned Start Time", Iso8601UtcNow));
            AddSpecialMapping("PlannedEndDateTime", () => new DefaultValueFieldMapping<DateTime>("Planned End Time", Iso8601UtcNow));
            AddSpecialMapping("ActivityId", () => new ValidatedModelFieldMapping("ActivityId", StringIsNotNullOrEmpty));

            AddRequiredMapping("PlannedStartDateTime", () => new RequiredFieldMapping<DateTime>("Planned Start Time", Iso8601UtcNow));
            AddRequiredMapping("PlannedEndDateTime", () => new RequiredFieldMapping<DateTime>("Planned End Time", Iso8601UtcNow));

            AddSupportedOperation(ViewAllowedOperations.AddRecord);
            AddSupportedOperation(ViewAllowedOperations.DeleteRecord);
            AddSupportedOperation(ViewAllowedOperations.ModifyRecord);
        }
    }
}