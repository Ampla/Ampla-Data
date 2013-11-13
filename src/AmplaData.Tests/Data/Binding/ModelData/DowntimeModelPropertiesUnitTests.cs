using AmplaData.Data.Attributes;
using NUnit.Framework;

namespace AmplaData.Data.Binding.ModelData
{
    [TestFixture]
    public class DowntimeModelPropertiesUnitTests : TestFixture
    {
        [AmplaLocation(Location = "Plant.Area.Downtime")]
        [AmplaModule(Module = "Downtime")]
        public class StringDowntimeModel 
        {
            public int Id { get; set; }
            public string Cause { get; set; }
            public string Classification { get; set; }
        }

        [AmplaLocation(Location = "Plant.Area.Downtime")]
        [AmplaModule(Module = "Downtime")]
        public class IntDowntimeModel
        {
            public int Id { get; set; }
            public int Cause { get; set; }
            public int Classification { get; set; }
        }

        [Test]
        public void IdModelProperties()
        {
            ModelProperties<IntDowntimeModel> modelProperties = new ModelProperties<IntDowntimeModel>();
            Assert.That(modelProperties.ResolveIdentifiers, Is.EqualTo(false));
        }

        [Test]
        public void StringModelProperties()
        {
            ModelProperties<StringDowntimeModel> modelProperties = new ModelProperties<StringDowntimeModel>();
            Assert.That(modelProperties.ResolveIdentifiers, Is.EqualTo(true));
        }

    }
}