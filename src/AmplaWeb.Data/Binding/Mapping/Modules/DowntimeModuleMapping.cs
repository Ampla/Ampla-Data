using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Binding.Mapping.Modules
{
    public class DowntimeModuleMapping : StandardModuleMapping
    {
        public DowntimeModuleMapping()
        {
            // Add Special Field Mappings for Downtime fields
            AddSpecialMapping("StartDateTime", () => new DefaultValueFieldMapping("Start Time", Iso8601UtcNow));
            AddSpecialMapping("Cause Location", () => new ValidatedModelFieldMapping("Cause Location", StringIsNotNullOrEmpty));
            AddSpecialMapping("Cause", () => new ValidatedModelFieldMapping("Cause", IsValidIdValue)); 
            AddSpecialMapping("Classification", () => new ValidatedModelFieldMapping("Classification", IsValidIdValue));

            // Add a required field mapping for Downtime fields
            AddRequiredMapping("StartDateTime", () => new DefaultValueFieldMapping("Start Time", Iso8601UtcNow));

            AddAllowedOperation(ViewAllowedOperations.AddRecord);
            AddAllowedOperation(ViewAllowedOperations.DeleteRecord);
            AddAllowedOperation(ViewAllowedOperations.ModifyRecord);
            
            AddAllowedOperation(ViewAllowedOperations.ConfirmRecord);
            AddAllowedOperation(ViewAllowedOperations.UnconfirmRecord);
        
            AddAllowedOperation(ViewAllowedOperations.SplitRecord);
        }
    }
}