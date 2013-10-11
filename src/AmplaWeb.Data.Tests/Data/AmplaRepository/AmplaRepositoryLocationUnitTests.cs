using System;
using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Production;
using AmplaWeb.Data.Records;
using AmplaWeb.Data.Views;
using NUnit.Framework;

namespace AmplaWeb.Data.AmplaRepository
{
    [TestFixture]
    public class AmplaRepositoryLocationUnitTests : AmplaRepositoryTestFixture<AmplaRepositoryLocationUnitTests.LocationModel>
    {

        [AmplaLocation(Location="Enterprise", WithRecurse = true)]
        [AmplaModule(Module = "Production")]
        public class LocationModel
        {
            public int Id { get; set; }

            [AmplaField(Field = "Sample Period")]
            public DateTime Sample { get; set; }

            public string Location { get; set; }
        }

        private const string module = "Production";
        private static readonly string[] Locations =  new [] {"Enterprise.Site.Area.Point1", "Enterprise.Site.Area.Point2"} ;

        public AmplaRepositoryLocationUnitTests() : base(module, Locations, ProductionViews.StandardView)
        {
        }

        [Test]
        public void AddAValidLocation()
        {
            LocationModel model = new LocationModel {Location = Locations[1]};
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(Locations[1]));
            Assert.That(record.Module, Is.EqualTo("Production"));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
        }

        [Test]
        public void AddWithoutALocation()
        {
            LocationModel model = new LocationModel();
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => Repository.Add(model));
            Assert.That(exception.ToString(), Is.StringContaining("Location '' is not valid."));
        }

        [Test]
        public void AddWithAnInvalidLocation()
        {
            LocationModel model = new LocationModel {Location = "Invalid.Location"};
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => Repository.Add(model));
            Assert.That(exception.ToString(), Is.StringContaining("Location 'Invalid.Location' is not valid."));
        }

        [Test]
        public void UpdateToChangeLocation()
        {
            LocationModel model = new LocationModel {Location = Locations[0]};
            Repository.Add(model);

            LocationModel existing = Repository.FindById(model.Id);
            Assert.That(existing, Is.Not.Null);

            existing.Location = Locations[1];

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => Repository.Update(existing));
            Assert.That(exception.ToString(), Is.StringContaining("The Location property is not the required value: "));
        }

        [Test]
        public void GetAll()
        {
            Assert.DoesNotThrow(() => Repository.GetAll());
        }
        
        [Test]
        public void FindByFilter()
        {
            Assert.DoesNotThrow(() => Repository.FindByFilter(null));
        }

        [Test]
        public void GetViaModel()
        {
            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(+1);

            LocationModel model = new LocationModel { Location = Locations[1] };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(Locations[1]));
            Assert.That(record.Module, Is.EqualTo("Production"));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.InRange(before.ToUniversalTime(), after.ToUniversalTime()));

            LocationModel getModel = Repository.FindById(model.Id);
            Assert.That(getModel.Sample, Is.InRange(before, after));
            Assert.That(getModel.Location, Is.EqualTo(Locations[1]));
        }

        [Test]
        public void GetViaRecord()
        {
            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(+1);

            InMemoryRecord record =ProductionRecords.NewRecord();
            record.Location = Locations[0];

            int recordId = SaveRecord(record);

            Assert.That(recordId, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            LocationModel getModel = Repository.FindById(recordId);
            Assert.That(getModel.Sample, Is.InRange(before, after));
            Assert.That(getModel.Location, Is.EqualTo(Locations[0]));
        }

    }
}