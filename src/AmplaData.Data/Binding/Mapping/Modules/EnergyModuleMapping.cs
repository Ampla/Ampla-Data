using AmplaData.Data.AmplaData2008;

namespace AmplaData.Data.Binding.Mapping.Modules
{
    public class EnergyModuleMapping : StandardModuleMapping
    {
        public EnergyModuleMapping()
        {
            // Add Special Field Mappings for Energy fields
            AddSpecialMapping("StartDateTime", () => new DefaultValueFieldMapping("Start Time", Iso8601UtcNow));
            AddSpecialMapping("Cause Location", () => new ValidatedModelFieldMapping("Cause Location", StringIsNotNullOrEmpty));
            AddSpecialMapping("Cause", () => new ValidatedModelFieldMapping("Cause", IsValidIdValue)); 
            AddSpecialMapping("Classification", () => new ValidatedModelFieldMapping("Classification", IsValidIdValue));

            // Add a required field mapping for Energy fields
            AddRequiredMapping("StartDateTime", () => new DefaultValueFieldMapping("Start Time", Iso8601UtcNow));

            AddSupportedOperation(ViewAllowedOperations.AddRecord);
            AddSupportedOperation(ViewAllowedOperations.DeleteRecord);
            AddSupportedOperation(ViewAllowedOperations.ModifyRecord);
            
            AddSupportedOperation(ViewAllowedOperations.ConfirmRecord);
            AddSupportedOperation(ViewAllowedOperations.UnconfirmRecord);
        
            AddSupportedOperation(ViewAllowedOperations.SplitRecord);
        }
    }
}