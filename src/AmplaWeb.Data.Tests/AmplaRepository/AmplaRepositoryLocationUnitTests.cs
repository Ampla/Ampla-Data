using System;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Records;
using AmplaWeb.Data.Views;
using NUnit.Framework;

namespace AmplaWeb.Data.AmplaRepository
{
    [TestFixture]
    public class AmplaRepositoryLocationUnitTests : AmplaRepositoryTestFixture<AmplaRepositoryLocationUnitTests.LocationModel>
    {

        [AmplaLocation(Location="Enterprise")]
        [AmplaModule(Module = "Production")]
        public class LocationModel
        {
            public int Id { get; set; }

            [AmplaField(Field = "Sample Period")]
            public DateTime Sample { get; set; }

            public string Location { get; set; }
        }

        private const string module = "Production";
        private const string location = "Enterprise";

        public AmplaRepositoryLocationUnitTests() : base(module, location, ProductionViews.StandardView)
        {
        }

        [Test]
        public void SubmitADifferentLocation()
        {
            LocationModel model = new LocationModel {Location = "Enterprise.Site.Point"};
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo("Enterprise.Site.Point"));
            Assert.That(record.Module, Is.EqualTo("Production"));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
        }

        [Test]
        public void SubmitADefaultLocation()
        {
            LocationModel model = new LocationModel { };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo("Enterprise"));
            Assert.That(record.Module, Is.EqualTo("Production"));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
        }
    }
}