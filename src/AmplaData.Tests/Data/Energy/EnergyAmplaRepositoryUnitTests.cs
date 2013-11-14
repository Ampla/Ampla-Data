using System;
using AmplaData.AmplaRepository;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.Energy
{
    [TestFixture]
    public class EnergyAmplaRepositoryUnitTests : AmplaRepositoryTestFixture<SimpleEnergyModel>
    {
        private const string module = "Energy";
        private const string location = "Enterprise.Site.Area.Energy";

        public EnergyAmplaRepositoryUnitTests() : base(module, location, EnergyViews.StandardView)
        {
        }

        [Test]
        public void SubmitWithNullFields()
        {
            SimpleEnergyModel model = new SimpleEnergyModel
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
            SimpleEnergyModel model = new SimpleEnergyModel { Location = location, StartTime = DateTime.Now, CauseLocation = "Enterprise.Site" };
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
            SimpleEnergyModel model = new SimpleEnergyModel { Location = location, StartTime = DateTime.Now, Cause = "Broken"};
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
            SimpleEnergyModel model = new SimpleEnergyModel { Location = location, StartTime = DateTime.Now, Classification = "Unplanned Process" };
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

            SimpleEnergyModel model = new SimpleEnergyModel { Location = location};
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

            int recordId = SaveRecord(EnergyRecords.NewRecord().MarkAsNew());
            Assert.That(recordId, Is.GreaterThan(1000));

            Assert.That(Records, Is.Not.Empty);

            SimpleEnergyModel model = Repository.FindById(recordId);
            Assert.That(model, Is.Not.Null);

            Assert.That(model.Location, Is.EqualTo(location));
            Assert.That(model.StartTime, Is.GreaterThan(before).And.LessThan(after));
            Assert.That(model.Duration, Is.EqualTo(90));
        }

    }
}