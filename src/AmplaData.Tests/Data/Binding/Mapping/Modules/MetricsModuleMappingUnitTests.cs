using AmplaData.Data.AmplaData2008;
using AmplaData.Data.Metrics;
using NUnit.Framework;

namespace AmplaData.Data.Binding.Mapping.Modules
{
    [TestFixture]
    public class MetricsModuleMappingUnitTests : ModuleMappingTestFixture
    {
        public MetricsModuleMappingUnitTests()
            : base(MetricsViews.StandardView, () => new MetricsModuleMapping())
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
            CheckField<ReadOnlyFieldMapping>("StartDateTime", "Start Time", true, false);
        }

        [Test]
        public void EndDateTime()
        {
            CheckField<ReadOnlyFieldMapping>("EndDateTime", "End Time", true, false);
        }

        [Test]
        public void Period()
        {
            CheckField<ReadOnlyFieldMapping>("Period", "Period", true, false);
        }

        [Test]
        public void Duration()
        {
            CheckField<ReadOnlyFieldMapping>("Duration", "Duration", true, false);
        }

        [Test]
        public void SupportedOperations()
        {
            CheckAllowedOperations(
                ViewAllowedOperations.ViewRecord);
        }
    }
}