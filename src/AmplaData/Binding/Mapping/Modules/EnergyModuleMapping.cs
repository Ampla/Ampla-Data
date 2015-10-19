using System;
using AmplaData.AmplaData2008;

namespace AmplaData.Binding.Mapping.Modules
{
    public class EnergyModuleMapping : StandardModuleMapping
    {
        public EnergyModuleMapping()
        {
            // Add Special Field Mappings for Energy fields
            AddSpecialMapping("StartDateTime", () => new DefaultValueFieldMapping<DateTime>("Start Time", Iso8601UtcNow));
            AddSpecialMapping("Cause Location", () => new ValidatedModelFieldMapping("Cause Location", StringIsNotNullOrEmpty));
            AddSpecialMapping("Cause", () => new ValidatedModelFieldMapping("Cause", IsValidIdValue)); 
            AddSpecialMapping("Classification", () => new ValidatedModelFieldMapping("Classification", IsValidIdValue));

            // Add a required field mapping for Energy fields
            AddRequiredMapping("StartDateTime", () => new RequiredFieldMapping<DateTime>("Start Time", Iso8601UtcNow));

            AddSupportedOperation(ViewAllowedOperations.AddRecord);
            AddSupportedOperation(ViewAllowedOperations.DeleteRecord);
            AddSupportedOperation(ViewAllowedOperations.ModifyRecord);
            
            AddSupportedOperation(ViewAllowedOperations.ConfirmRecord);
            AddSupportedOperation(ViewAllowedOperations.UnconfirmRecord);
        
            AddSupportedOperation(ViewAllowedOperations.SplitRecord);
        }
    }
}