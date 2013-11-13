using System;
using AmplaData.Data.AmplaRepository;
using AmplaData.Data.Records;
using AmplaData.Data.Views;
using NUnit.Framework;

namespace AmplaData.Data.Downtime
{
    [TestFixture]
    public class ResolveIdentifiersAmplaRepositoryUnitTests : AmplaRepositoryTestFixture<IdentifierDowntimeModel>
    {
        private const string module = "Downtime";
        private const string location = "Enterprise.Site.Area.Downtime";

        public ResolveIdentifiersAmplaRepositoryUnitTests() : base(module, location, DowntimeViews.StandardView)
        {
        }

        [Test]
        public void SubmitWithNullFields()
        {
            IdentifierDowntimeModel model = new IdentifierDowntimeModel()
                {
                    Location = location,
                    StartTime = DateTime.Now,
                };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Module, Is.EqualTo(module));
            Assert.That(record.GetFieldValue("Start Time", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
            Assert.That(record.Find("Cause Location"), Is.Null);
            Assert.That(record.Find("Cause"), Is.Null);
            Assert.That(record.Find("Classification"), Is.Null);
        }

        [Test]
        public void SubmitWithValidCauseLocation()
        {
            IdentifierDowntimeModel model = new IdentifierDowntimeModel { Location = location, StartTime = DateTime.Now, CauseLocation = "Enterprise.Site" };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Module, Is.EqualTo(module));
            Assert.That(record.GetFieldValue("Start Time", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
            Assert.That(record.GetFieldValue("Cause Location", string.Empty), Is.EqualTo("Enterprise.Site"));
        }

        [Test]
        public void SubmitWithCause()
        {
            IdentifierDowntimeModel model = new IdentifierDowntimeModel { Location = location, StartTime = DateTime.Now, Cause = 100};
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.GetFieldValue("Cause", -1), Is.EqualTo(100));
        }

        [Test]
        public void SubmitWithClassification()
        {
            IdentifierDowntimeModel model = new IdentifierDowntimeModel { Location = location, StartTime = DateTime.Now, Classification = 200 };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.GetFieldValue("Classification", -1), Is.EqualTo(200));
        }

        [Test]
        public void LoadModelWithIds()
        {
            InMemoryRecord record = DowntimeRecords.NewRecord().MarkAsNew();
            Assert.That(record.Location, Is.EqualTo(location));

            record.SetFieldValue("Cause Location", "Plant.Area");
            record.SetFieldIdValue("Cause", "Shutdown", 100);
            record.SetFieldIdValue("Classification", "Unplanned Process", 200);
            SaveRecord(record);

            Assert.That(Records, Is.Not.Empty);

            int recordId = Records[0].RecordId;
            Assert.That(recordId, Is.GreaterThan(0));

            IdentifierDowntimeModel model = Repository.FindById(recordId);

            Assert.That(model, Is.Not.Null);

            Assert.That(model.Cause, Is.EqualTo(100));
            Assert.That(model.Classification, Is.EqualTo(200));
        }

        [Test]
        public void LoadModelWithStrings()
        {
            InMemoryRecord record = DowntimeRecords.NewRecord().MarkAsNew();
            Assert.That(record.Location, Is.EqualTo(location));

            record.SetFieldValue("Cause Location", "Plant.Area");
            record.SetFieldIdValue("Cause", "Shutdown", 100);
            record.SetFieldIdValue("Classification", "Unplanned Process", 200);
            SaveRecord(record);

            Assert.That(Records, Is.Not.Empty);

            int recordId = Records[0].RecordId;
            Assert.That(recordId, Is.GreaterThan(0));

            IdentifierDowntimeModel model = Repository.FindById(recordId);

            Assert.That(model, Is.Not.Null);

            Assert.That(model.Cause, Is.EqualTo(100));
            Assert.That(model.Classification, Is.EqualTo(200));
        }

    }
}