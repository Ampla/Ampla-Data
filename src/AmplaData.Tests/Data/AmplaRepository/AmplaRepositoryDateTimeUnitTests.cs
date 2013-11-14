using System;
using AmplaData.Attributes;
using AmplaData.Production;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.AmplaRepository
{
    [TestFixture]
    public class AmplaRepositoryDateTimeUnitTests :
        AmplaRepositoryTestFixture<AmplaRepositoryDateTimeUnitTests.AreaValueModel>
    {
        private const string module = "Production";
        private const string location = "Enterprise.Site.Area.Production";

        [AmplaLocation(Location = location)]
        [AmplaModule(Module = module)]
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

        [Test]
        public void GetVersionsSingleRecord()
        {
            Assert.That(Records, Is.Empty);

            DateTime localHour = DateTime.Now.TrimToHour();

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100, Sample = localHour };

            Repository.Add(model);

            ModelVersions versions = Repository.GetVersions(model.Id);
            
            Assert.That(versions, Is.Not.Null);
            Assert.That(versions.Versions, Is.Not.Empty);
            Assert.That(versions.Versions.Count, Is.EqualTo(1));

            ModelVersion<AreaValueModel> version = (ModelVersion<AreaValueModel>) versions.Versions[0];

            Assert.That(version.Display, Is.EqualTo("User created record"));
            Assert.That(version.Model.Id, Is.EqualTo(model.Id));
            Assert.That(version.Model.Area, Is.EqualTo(model.Area));
            Assert.That(version.Model.Sample, Is.EqualTo(model.Sample));
            Assert.That(version.Model.Value, Is.EqualTo(model.Value));

            AmplaAuditRecord history = Repository.GetHistory(model.Id);

            Assert.That(history.Changes, Is.Empty);
        }

        [Test]
        public void GetVersionsWithEdit()
        {
            Assert.That(Records, Is.Empty);

            DateTime localHour = DateTime.Now.TrimToHour();

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100, Sample = localHour };

            Repository.Add(model);

            AreaValueModel update = new AreaValueModel {Id = model.Id, Area = "ROM", Value = 100, Sample = model.Sample.AddMinutes(1)};
            Repository.Update(update);

            ModelVersions versions = Repository.GetVersions(model.Id);

            AmplaAuditRecord history = Repository.GetHistory(model.Id);

            Assert.That(versions, Is.Not.Null);
            Assert.That(versions.Versions, Is.Not.Empty);
            Assert.That(versions.Versions.Count, Is.EqualTo(2));

            ModelVersion<AreaValueModel> last = (ModelVersion<AreaValueModel>)versions.Versions[0];
            ModelVersion<AreaValueModel> current = (ModelVersion<AreaValueModel>)versions.Versions[1];

            Assert.That(last.Display, Is.EqualTo("User created record"));
            Assert.That(last.Model.Id, Is.EqualTo(model.Id));
            Assert.That(last.Model.Area, Is.EqualTo(model.Area));
            Assert.That(last.Model.Sample, Is.EqualTo(model.Sample));
            Assert.That(last.Model.Value, Is.EqualTo(model.Value));

            Assert.That(current.Display, Is.EqualTo("User modified record (Sample Period)"));
            Assert.That(current.Model.Id, Is.EqualTo(update.Id));
            Assert.That(current.Model.Area, Is.EqualTo(update.Area));
            Assert.That(current.Model.Sample, Is.EqualTo(update.Sample));
            Assert.That(current.Model.Value, Is.EqualTo(update.Value));

            Assert.That(history.Changes, Is.Not.Empty);
            Assert.That(history.Changes[0].Fields[0].Name, Is.EqualTo("SampleDateTime"));
        }

    }
}