using System;
using AmplaData.AmplaRepository;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.Planning
{
    [TestFixture]
    public class PlanningAmplaRepositoryUnitTests : AmplaRepositoryTestFixture<SimplePlanningModel>
    {
        private const string module = "Planning";
        private const string location = "Enterprise.Site.Area.Planning";

        public PlanningAmplaRepositoryUnitTests()
            : base(module, location, PlanningViews.StandardView)
        {
        }

        [Test]
        public void SubmitWithBasicFields()
        {
            SimplePlanningModel model = new SimplePlanningModel
                {
                    Location = location,
                    PlannedStart = DateTime.Today,
                    PlannedEnd = DateTime.Today.AddDays(1),
                    State = "Available",
                    ActivityId = "ABC-123"
                };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Module, Is.EqualTo(module));
            Assert.That(record.GetFieldValue("Planned Start Time", DateTime.MinValue), Is.EqualTo(DateTime.Today.ToUniversalTime()));
            Assert.That(record.GetFieldValue("Planned End Time", DateTime.MinValue), Is.EqualTo(DateTime.Today.AddDays(1).ToUniversalTime()));
            Assert.That(record.Find("State"), Is.Null);
            Assert.That(record.GetFieldValue("ActivityId", ""), Is.EqualTo("ABC-123"));
        }

        [Test]
        public void DefaultStartTimeAndEndTime()
        {
            SimplePlanningModel model = new SimplePlanningModel { Location = location };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.GetFieldValue("Planned Start Time", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
            Assert.That(record.GetFieldValue("Planned End Time", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
        }

        [Test]
        public void GetFromRecord()
        {
            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(+1);

            int recordId = SaveRecord(PlanningRecords.NewRecord().MarkAsNew());
            Assert.That(recordId, Is.GreaterThan(1000));

            Assert.That(Records, Is.Not.Empty);

            SimplePlanningModel model = Repository.FindById(recordId);
            Assert.That(model, Is.Not.Null);

            Assert.That(model.Location, Is.EqualTo(location));
            Assert.That(model.PlannedStart, Is.InRange(before, after));
            Assert.That(model.PlannedEnd, Is.InRange(before.AddHours(1), after.AddHours(1)));

        }

        [Test]
        public void NullActivityId()
        {
            SimplePlanningModel model = new SimplePlanningModel { Location = location };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Find("ActivityId"), Is.Null);
            Assert.That(record.GetFieldValue("Planned Start Time", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
            Assert.That(record.GetFieldValue("Planned End Time", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
        }


        [Test]
        public void WithActivityId()
        {
            SimplePlanningModel model = new SimplePlanningModel { Location = location, ActivityId = "New Activity"};
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.GetFieldValue("ActivityId", ""), Is.EqualTo("New Activity"));
            Assert.That(record.GetFieldValue("Planned Start Time", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
            Assert.That(record.GetFieldValue("Planned End Time", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
        }

        [Test]
        public void WithEmptyActivityId()
        {
            SimplePlanningModel model = new SimplePlanningModel { Location = location, ActivityId = string.Empty };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Find("ActivityId"), Is.Null);
            Assert.That(record.GetFieldValue("Planned Start Time", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
            Assert.That(record.GetFieldValue("Planned End Time", DateTime.MinValue), Is.GreaterThan(DateTime.MinValue));
        }

    }
}