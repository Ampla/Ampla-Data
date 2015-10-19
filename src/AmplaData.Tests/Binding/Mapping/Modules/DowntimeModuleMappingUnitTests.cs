using System;
using AmplaData.AmplaData2008;
using AmplaData.Modules.Downtime;
using NUnit.Framework;

namespace AmplaData.Binding.Mapping.Modules
{
    [TestFixture]
    public class DowntimeModuleMappingUnitTests : ModuleMappingTestFixture
    {
        public DowntimeModuleMappingUnitTests() : base(DowntimeViews.StandardView, ()=> new DowntimeModuleMapping())
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
            CheckSpecialField<DefaultValueFieldMapping<DateTime>>("StartDateTime", "Start Time");
            CheckRequiredField<RequiredFieldMapping<DateTime>>("StartDateTime", "Start Time");
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