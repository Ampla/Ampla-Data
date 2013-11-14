using AmplaData.AmplaData2008;
using AmplaData.Planning;
using NUnit.Framework;

namespace AmplaData.Binding.Mapping.Modules
{
    [TestFixture]
    public class PlanningModuleMappingUnitTests : ModuleMappingTestFixture
    {
        public PlanningModuleMappingUnitTests() : base(PlanningViews.StandardView, () => new PlanningModuleMapping())
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
        public void PlannedStartDateTime()
        {
            CheckField<DefaultValueFieldMapping>("PlannedStartDateTime", "Planned Start Time", true, true);
        }

        [Test]
        public void PlannedEndDateTime()
        {
            CheckField<DefaultValueFieldMapping>("PlannedEndDateTime", "Planned End Time", true, true);
        }

        [Test]
        public void State()
        {
            CheckField<ReadOnlyFieldMapping>("State", "State", true, false);
        }

        [Test]
        public void SupportedOperations()
        {
            CheckAllowedOperations(
                ViewAllowedOperations.AddRecord,
                ViewAllowedOperations.DeleteRecord,
                ViewAllowedOperations.ModifyRecord,
                ViewAllowedOperations.ViewRecord);
        }
    }
}