using AmplaData.Data.AmplaData2008;
using AmplaData.Data.Binding.ViewData;
using AmplaData.Data.Energy;
using NUnit.Framework;

namespace AmplaData.Data.Binding.Mapping.Modules
{
    [TestFixture]
    public class EnergyModuleMappingUnitTests : ModuleMappingTestFixture
    {
        public EnergyModuleMappingUnitTests() : base(EnergyViews.StandardView, ()=> new EnergyModuleMapping())
        {
        }

        [Test]
        public void IdField()
        {
            CheckField<IdFieldMapping>("Id", "Id", true, false);
        }
        
        [Test]
        public void LocationField()
        {
            CheckField<ReadOnlyFieldMapping>("ObjectId", "Location", true, false);
        }

        [Test]
        public void StartDateTime()
        {
            CheckField<DefaultValueFieldMapping>("StartDateTime", "Start Time", true, true);
        }

        [Test]
        public void EndDateTime()
        {
            CheckField<ModelFieldMapping>("EndDateTime", "End Time", true, false);
        }

        [Test]
        public void CauseLocation()
        {
            CheckField<ValidatedModelFieldMapping>("Cause Location", "Cause Location", true, false);
        }

        [Test]
        public void Cause()
        {
            CheckField<ValidatedModelFieldMapping>("Cause", "Cause", true, false);
        }

        [Test]
        public void Classification()
        {
            CheckField<ValidatedModelFieldMapping>("Classification", "Classification", true, false);
        }

        [Test]
        public void SupportedOperations()
        {
            CheckAllowedOperations(
                ViewAllowedOperations.AddRecord, 
                ViewAllowedOperations.ConfirmRecord,
                ViewAllowedOperations.DeleteRecord, 
                ViewAllowedOperations.ModifyRecord,
                ViewAllowedOperations.SplitRecord, 
                ViewAllowedOperations.UnconfirmRecord,
                ViewAllowedOperations.ViewRecord);
        }
 
    }
}