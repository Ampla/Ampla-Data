using System;
using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Production;
using AmplaWeb.Data.Records;
using AmplaWeb.Data.Views;
using NUnit.Framework;

namespace AmplaWeb.Data.AmplaRepository
{
    [TestFixture]
    public class AmplaRepositoryDateTimeUnitTests :
        AmplaRepositoryTestFixture<AmplaRepositoryDateTimeUnitTests.AreaValueModel>
    {
        private const string module = "Production";
        private const string location = "Plant.Area.Values";

        [AmplaLocation(Location = "Plant.Area.Values")]
        [AmplaModule(Module = "Production")]
        public class AreaValueModel
        {
            public int Id { get; set; }

            [AmplaField(Field = "Sample Period")]
            public DateTime Sample { get; set; }

            public string Area { get; set; }

            public double Value { get; set; }
        }

        public AmplaRepositoryDateTimeUnitTests()
            : base(module, location, ProductionViews.AreaValueModelView)
        {
        }

        [Test]
        public void RecordUsesUtcTime()
        {
            Assert.That(Records, Is.Empty);

            DateTime beforeUtc = DateTime.UtcNow.AddMinutes(-5);
            DateTime afterUtc = DateTime.UtcNow.AddMinutes(+5);

            DateTime beforeLocal = beforeUtc.ToLocalTime();
            DateTime afterLocal = afterUtc.ToLocalTime();

            AreaValueModel model = new AreaValueModel {Area = "ROM", Value = 100};

            Repository.Add(model);
            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.GetFieldValue("Area", ""), Is.EqualTo("ROM"));
            Assert.That(record.GetFieldValue<double>("Value", 0), Is.EqualTo(100.0d));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue),
                        Is.GreaterThan(beforeUtc).And.LessThan(afterUtc));

            Assert.That(model.Id, Is.EqualTo(record.RecordId));

            AreaValueModel updated = Repository.FindById(record.RecordId);

            Assert.That(updated.Sample, Is.GreaterThan(beforeLocal).And.LessThan(afterLocal));
        }

        [Test]
        public void ModelsUseLocalTime()
        {
            Assert.That(Records, Is.Empty);

            DateTime localHour = DateTime.Now.TrimToHour();

            AreaValueModel model = new AreaValueModel {Area = "ROM", Value = 100, Sample = localHour};

            DateTime utcHour = localHour.ToUniversalTime();

            Repository.Add(model);
            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.GetFieldValue("Area", ""), Is.EqualTo("ROM"));
            Assert.That(record.GetFieldValue<double>("Value", 0), Is.EqualTo(100.0d));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.EqualTo(utcHour));

            Assert.That(model.Id, Is.EqualTo(record.RecordId));

            AreaValueModel updated = Repository.FindById(record.RecordId);

            Assert.That(updated.Sample, Is.EqualTo(localHour));
        }

        [Test]
        public void AmplaRecordShowsLocalTime()
        {
            Assert.That(Records, Is.Empty);

            DateTime localHour = DateTime.Now.TrimToHour();

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100, Sample = localHour };

            Repository.Add(model);
            Assert.That(Records, Is.Not.Empty);

            int recordId = Records[0].RecordId;

            AmplaRecord record = Repository.FindRecord(recordId);

            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Id, Is.EqualTo(recordId));
            Assert.That(record.GetValue("Area"), Is.EqualTo("ROM"));
            Assert.That(record.GetValue("Value"), Is.EqualTo(100.0d));
            Assert.That(record.GetValue("Sample Period"), Is.EqualTo(localHour));
        }
    }
}