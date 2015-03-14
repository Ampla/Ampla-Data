using System;
using AmplaData.AmplaRepository;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.Modules.Metrics
{
    [TestFixture]
    public class MetricsAmplaRepositoryUnitTests : AmplaRepositoryTestFixture<SimpleMetricsModel>
    {
        private const string module = "Metrics";
        private const string location = "Enterprise.Site.Area.Metrics";

        public MetricsAmplaRepositoryUnitTests()
            : base(module, location, MetricsViews.TotalTonnesView)
        {
        }

        [Test]
        public void AddModelWillThrow()
        {
            SimpleMetricsModel model = new SimpleMetricsModel
                {
                    Location = location,
                    StartTime = DateTime.Today,
                    Duration = 600,
                    TotalTonnes = 100
                };

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => Repository.Add(model));
            Assert.That(exception.Message, Is.StringContaining("'AddRecord'"));
        }

        [Test]
        public void Get()
        {
            DateTime before = DateTime.Today.AddMinutes(-1);
            DateTime after = DateTime.Today.AddMinutes(+1);

            int recordId = SaveRecord(MetricsRecords.NewRecord().MarkAsNew());
            Assert.That(recordId, Is.GreaterThan(1000));

            Assert.That(Records, Is.Not.Empty);

            SimpleMetricsModel model = Repository.FindById(recordId);
            Assert.That(model, Is.Not.Null);

            Assert.That(model.Location, Is.EqualTo(location));
            Assert.That(model.StartTime, Is.GreaterThan(before).And.LessThan(after));
            Assert.That(model.Duration, Is.EqualTo(60 * 60));

        }

        [Test]
        public void UpdateModelWillThrow()
        {
            int recordId = SaveRecord(MetricsRecords.NewRecord().MarkAsNew());
            Assert.That(recordId, Is.GreaterThan(1000));

            Assert.That(Records, Is.Not.Empty);

            SimpleMetricsModel model = Repository.FindById(recordId);
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Location, Is.EqualTo(location));

            model.TotalTonnes += 100;

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => Repository.Update(model));
            Assert.That(exception.Message, Is.StringContaining("'ModifyRecord'"));
        }


        [Test]
        public void ConfirmModelWillThrow()
        {
            int recordId = SaveRecord(MetricsRecords.NewRecord().MarkAsNew());
            Assert.That(recordId, Is.GreaterThan(1000));

            Assert.That(Records, Is.Not.Empty);

            SimpleMetricsModel model = Repository.FindById(recordId);
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Location, Is.EqualTo(location));

            model.TotalTonnes += 100;

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => Repository.Confirm(model));
            Assert.That(exception.Message, Is.StringContaining("'ConfirmRecord'"));
        }

        [Test]
        public void UnconfirmModelWillThrow()
        {
            int recordId = SaveRecord(MetricsRecords.NewRecord().MarkAsNew());
            Assert.That(recordId, Is.GreaterThan(1000));

            Assert.That(Records, Is.Not.Empty);

            SimpleMetricsModel model = Repository.FindById(recordId);
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Location, Is.EqualTo(location));

            model.TotalTonnes += 100;

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => Repository.Unconfirm(model));
            Assert.That(exception.Message, Is.StringContaining("'UnconfirmRecord'"));
        }

        [Test]
        public void DeleteModelWillThrow()
        {
            int recordId = SaveRecord(MetricsRecords.NewRecord().MarkAsNew());
            Assert.That(recordId, Is.GreaterThan(1000));

            Assert.That(Records, Is.Not.Empty);

            SimpleMetricsModel model = Repository.FindById(recordId);
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Location, Is.EqualTo(location));

            model.TotalTonnes += 100;

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => Repository.Delete(model));
            Assert.That(exception.Message, Is.StringContaining("'DeleteRecord'"));
        }

    }
}