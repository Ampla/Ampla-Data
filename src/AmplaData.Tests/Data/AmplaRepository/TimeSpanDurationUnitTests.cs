using System;
using AmplaData.Attributes;
using AmplaData.Production;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.AmplaRepository
{
    [TestFixture]
    public class TimeSpanDurationUnitTests : AmplaRepositoryTestFixture<TimeSpanDurationUnitTests.TimeSpanModel>
    {
        [AmplaLocation(Location = "Enterprise.Site.Area.Production")]
        [AmplaModule(Module = "Production")]
        public class TimeSpanModel
        {
            public int Id { get; set; }

            [AmplaField(Field = "Sample Period")]
            public DateTime Sample { get; set; }

            public TimeSpan Duration { get; set; }
        }

        private const string module = "Production";
        private const string location = "Enterprise.Site.Area.Production";

        public TimeSpanDurationUnitTests() : base(module, location, ProductionViews.StandardView)
        {
        }

        [Test]
        public void AddDefaultModel()
        {
            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(1);

            TimeSpanModel model = new TimeSpanModel();
            Repository.Add(model);

            Assert.That(Records, Is.Not.Empty);
            var record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Module, Is.EqualTo(module));
            Assert.That(record.GetFieldValue("Duration", ""), Is.EqualTo("0"));
            Assert.That(record.RecordId, Is.GreaterThan(0));

            TimeSpanModel retrieved = Repository.FindById(record.RecordId);

            Assert.That(retrieved, Is.Not.Null);

            Assert.That(retrieved.Id, Is.EqualTo(record.RecordId));
            Assert.That(retrieved.Sample, Is.InRange(before, after));
            Assert.That(retrieved.Duration, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void AddWithDuration()
        {
            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(1);

            TimeSpanModel model = new TimeSpanModel {Duration = TimeSpan.FromMinutes(60)};
            Repository.Add(model);

            Assert.That(Records, Is.Not.Empty);
            var record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Module, Is.EqualTo(module));
            Assert.That(record.GetFieldValue("Duration", -1), Is.EqualTo(60 * 60));
            Assert.That(record.RecordId, Is.GreaterThan(0));

            TimeSpanModel retrieved = Repository.FindById(record.RecordId);

            Assert.That(retrieved, Is.Not.Null);

            Assert.That(retrieved.Id, Is.EqualTo(record.RecordId));
            Assert.That(retrieved.Sample, Is.InRange(before, after));
            Assert.That(retrieved.Duration, Is.EqualTo(TimeSpan.FromMinutes(60)));
        }

        [Test]
        public void GetWithZeroDuration()
        {
            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(1);

            var record = ProductionRecords.NewRecord().MarkAsNew();
            record.SetFieldValue("Duration", 0);

            int recordId = SaveRecord(record);

            Assert.That(Records, Is.Not.Empty);
            
            TimeSpanModel retrieved = Repository.FindById(recordId);

            Assert.That(retrieved, Is.Not.Null);

            Assert.That(retrieved.Id, Is.EqualTo(recordId));
            Assert.That(retrieved.Sample, Is.InRange(before, after));
            Assert.That(retrieved.Duration, Is.EqualTo(TimeSpan.FromMinutes(0)));
        }

        [Test]
        public void GetWithDuration()
        {
            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(1);

            var record = ProductionRecords.NewRecord();
            record.SetFieldValue("Duration", 60);

            int recordId = SaveRecord(record);

            Assert.That(Records, Is.Not.Empty);

            TimeSpanModel retrieved = Repository.FindById(recordId);

            Assert.That(retrieved, Is.Not.Null);

            Assert.That(retrieved.Id, Is.EqualTo(recordId));
            Assert.That(retrieved.Sample, Is.InRange(before, after));
            Assert.That(retrieved.Duration, Is.EqualTo(TimeSpan.FromMinutes(1)));
        }
    }
}