using System;
using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Records;
using AmplaWeb.Data.Views;
using NUnit.Framework;

namespace AmplaWeb.Data.AmplaRepository
{
    [TestFixture]
    public class DowntimeAmplaRepositoryUnitTests : AmplaRepositoryTestFixture<DowntimeAmplaRepositoryUnitTests.SimpleDowntimeModel>
    {
        [AmplaLocation(Location = "Enterprise.Site.Area.Point")]
        [AmplaModule(Module = "Downtime")]
        public class SimpleDowntimeModel
        {
            public int Id { get; set; }
            public string Location { get; set; }

            [AmplaField(Field="Start Time")]
            public DateTime StartTime { get; set; }

            [AmplaField(Field = "Cause Location")]
            public string CauseLocation { get; set; }

            public string Cause { get; set; }
            
            public string Classification { get; set; }
        }

        private const string module = "Downtime";
        private const string location = "Enterprise.Site.Area.Point";

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

        [Test]
        public void SubmitWithCause()
        {
            SimpleDowntimeModel model = new SimpleDowntimeModel { Location = location, StartTime = DateTime.Now, Cause = "Broken"};
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.GetFieldValue("Cause", string.Empty), Is.EqualTo("Broken"));
        }

        [Test]
        public void SubmitWithClassification()
        {
            SimpleDowntimeModel model = new SimpleDowntimeModel { Location = location, StartTime = DateTime.Now, Classification = "Unplanned Process" };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.GetFieldValue("Classification", string.Empty), Is.EqualTo("Unplanned Process"));
        }

    }
}