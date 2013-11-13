using System;
using AmplaData.Data.AmplaRepository;
using AmplaData.Data.Records;
using NUnit.Framework;

namespace AmplaData.Data.Maintenance
{
    [TestFixture]
    public class MaintenanceAmplaRepositoryUnitTests : AmplaRepositoryTestFixture<SimpleMaintenanceModel>
    {
        private const string module = "Maintenance";
        private const string location = "Enterprise.Site.Area.Maintenance";

        public MaintenanceAmplaRepositoryUnitTests()
            : base(module, location, MaintenanceViews.StandardView)
        {
        }

        [Test]
        public void SubmitWithBasicFields()
        {
            SimpleMaintenanceModel model = new SimpleMaintenanceModel
                {
                    Location = location,
                    SamplePeriod = DateTime.Today,
                    Duration = 600
                };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Module, Is.EqualTo(module));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.EqualTo(DateTime.Today.ToUniversalTime()));
            Assert.That(record.GetFieldValue("Duration", 0), Is.EqualTo(600));
        }

        [Test]
        public void DefaultStartTime()
        {
            SimpleMaintenanceModel model = new SimpleMaintenanceModel { Location = location };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
        }

        [Test]
        public void Get()
        {
            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(+1);

            int recordId = SaveRecord(MaintenanceRecords.NewRecord().MarkAsNew());
            Assert.That(recordId, Is.GreaterThan(1000));

            Assert.That(Records, Is.Not.Empty);

            SimpleMaintenanceModel model = Repository.FindById(recordId);
            Assert.That(model, Is.Not.Null);

            Assert.That(model.Location, Is.EqualTo(location));
            Assert.That(model.SamplePeriod, Is.GreaterThan(before).And.LessThan(after));
            Assert.That(model.Duration, Is.EqualTo(90));

        }

    }
}