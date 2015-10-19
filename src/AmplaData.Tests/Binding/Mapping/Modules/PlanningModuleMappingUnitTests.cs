using System;
using AmplaData.AmplaData2008;
using AmplaData.Modules.Planning;
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
            CheckSpecialField<DefaultValueFieldMapping<DateTime>>("PlannedStartDateTime", "Planned Start Time");
            CheckRequiredField<RequiredFieldMapping<DateTime>>("PlannedStartDateTime", "Planned Start Time");
        }

        [Test]
        public void PlannedEndDateTime()
        {
            CheckSpecialField<DefaultValueFieldMapping<DateTime>>("PlannedEndDateTime", "Planned End Time");
            CheckRequiredField<RequiredFieldMapping<DateTime>>("PlannedEndDateTime", "Planned End Time");
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