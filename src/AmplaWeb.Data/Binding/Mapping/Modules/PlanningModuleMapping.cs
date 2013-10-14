﻿
using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Binding.Mapping.Modules
{
    public class PlanningModuleMapping : StandardModuleMapping
    {
        public PlanningModuleMapping()
        {
            AddSpecialMapping("PlannedStartDateTime", () => new DefaultValueFieldMapping("Planned Start Time", Iso8601UtcNow));
            AddSpecialMapping("PlannedEndDateTime", () => new DefaultValueFieldMapping("Planned End Time", Iso8601UtcNow));

            AddRequiredMapping("PlannedStartDateTime", () => new DefaultValueFieldMapping("Planned Start Time", Iso8601UtcNow));
            AddRequiredMapping("PlannedEndDateTime", () => new DefaultValueFieldMapping("Planned End Time", Iso8601UtcNow));

            AddSupportedOperation(ViewAllowedOperations.AddRecord);
            AddSupportedOperation(ViewAllowedOperations.DeleteRecord);
            AddSupportedOperation(ViewAllowedOperations.ModifyRecord);
        }
    }
}