using AmplaWeb.Data.Production;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.Mapping.Modules
{
    [TestFixture]
    public class ProductionModuleMappingUnitTests : ModuleMappingTestFixture
    {
        public ProductionModuleMappingUnitTests() : base(ProductionViews.StandardView, () => new ProductionModuleMapping())
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
            CheckField<DefaultValueFieldMapping>("SampleDateTime", "Sample Period", true, true);
        }
    }
}