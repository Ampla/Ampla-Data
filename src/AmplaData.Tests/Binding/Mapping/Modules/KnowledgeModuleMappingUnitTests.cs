using System;
using AmplaData.AmplaData2008;
using AmplaData.Modules.Knowledge;
using NUnit.Framework;

namespace AmplaData.Binding.Mapping.Modules
{
    [TestFixture]
    public class KnowledgeModuleMappingUnitTests : ModuleMappingTestFixture
    {
        public KnowledgeModuleMappingUnitTests() : base(KnowledgeViews.StandardView, () => new KnowledgeModuleMapping())
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
        public void SamplePeriod()
        {
            CheckSpecialField<DefaultValueFieldMapping<DateTime>>("SampleDateTime", "Sample Period");
            CheckRequiredField<RequiredFieldMapping<DateTime>>("SampleDateTime", "Sample Period");
        }

        [Test]
        public void SupportedOperations()
        {
            CheckAllowedOperations(
                ViewAllowedOperations.AddRecord,
                ViewAllowedOperations.ConfirmRecord,
                ViewAllowedOperations.DeleteRecord,
                ViewAllowedOperations.ModifyRecord,
                ViewAllowedOperations.UnconfirmRecord,
                ViewAllowedOperations.ViewRecord);
        }
    }
}