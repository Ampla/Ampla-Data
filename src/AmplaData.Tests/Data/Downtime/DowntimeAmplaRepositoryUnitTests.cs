using System;
using AmplaData.Data.AmplaRepository;
using AmplaData.Data.Records;
using NUnit.Framework;

namespace AmplaData.Data.Downtime
{
    [TestFixture]
    public class DowntimeAmplaRepositoryUnitTests : AmplaRepositoryTestFixture<SimpleDowntimeModel>
    {
        private const string module = "Downtime";
        private const string location = "Enterprise.Site.Area.Downtime";

        public DowntimeAmplaRepositoryUnitTests() : base(module, location, DowntimeViews.StandardView)
        {
        }

        [Test]
        public void SubmitWithNullFields()
        {
            SimpleDowntimeModel model = new SimpleDowntimeModel
                {
                    Location = location,
                    StartTime = DateTime.Now,
                    CauseLocation = null,
                    Cause = null,
                    Classification = null
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
            SimpleDowntimeModel model = new SimpleDowntimeModel { Location = location, StartTime = DateTime.Now, CauseLocation = "Enterprise.Site" };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Module, Is.EqualTo(module));
            Assert.That(record.GetFieldValue("Start Time", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
            Assert.That(record.GetFieldValue("Cause Location", string.Empty), Is.EqualTo("Enterprise.Site"));
        }

        /// <summary>
        /// Cause will read the value as string but will only write as a int
        /// </summary>
        [Test]
        public void SubmitWithCauseAsString()
        {
            SimpleDowntimeModel model = new SimpleDowntimeModel { Location = location, StartTime = DateTime.Now, Cause = "Broken"};
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Find("Cause"), Is.Null);
        }

        [Test]
        public void SubmitWithClassificationAsString()
        {
            SimpleDowntimeModel model = new SimpleDowntimeModel { Location = location, StartTime = DateTime.Now, Classification = "Unplanned Process" };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Find("Classification"), Is.Null);
        }

        [Test]
        public void DefaultStartTime()
        {
            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(+1);

            SimpleDowntimeModel model = new SimpleDowntimeModel { Location = location};
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Find("Sample Period"), Is.Null);
            Assert.That(record.GetFieldValue("Start Time", DateTime.MinValue), Is.InRange(before.ToUniversalTime(), after.ToUniversalTime()));
        }

        [Test]
        public void GetFromRecord()
        {
            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(+1);

            int recordId = SaveRecord(DowntimeRecords.NewRecord().MarkAsNew());
            Assert.That(recordId, Is.GreaterThan(1000));

            Assert.That(Records, Is.Not.Empty);

            SimpleDowntimeModel model = Repository.FindById(recordId);
            Assert.That(model, Is.Not.Null);

            Assert.That(model.Location, Is.EqualTo(location));
            Assert.That(model.StartTime, Is.GreaterThan(before).And.LessThan(after));
            Assert.That(model.Duration, Is.EqualTo(90));
        }

    }
}