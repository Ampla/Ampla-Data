using System;
using AmplaData.Data.AmplaRepository;
using AmplaData.Data.Records;
using AmplaData.Data.Views;
using NUnit.Framework;

namespace AmplaData.Data.Knowledge
{
    [TestFixture]
    public class KnowledgeAmplaRepositoryUnitTests : AmplaRepositoryTestFixture<SimpleKnowledgeModel>
    {
        private const string module = "Knowledge";
        private const string location = "Enterprise.Site.Area.Knowledge";

        public KnowledgeAmplaRepositoryUnitTests()
            : base(module, location, KnowledgeViews.StandardView)
        {
        }

        [Test]
        public void SubmitWithBasicFields()
        {
            SimpleKnowledgeModel model = new SimpleKnowledgeModel
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
            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(+1);

            SimpleKnowledgeModel model = new SimpleKnowledgeModel { Location = location };
            Repository.Add(model);

            Assert.That(model.Id, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.InRange(before.ToUniversalTime(), after.ToUniversalTime()));
        }

        [Test]
        public void GetViaRecord()
        {
            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(+1);

            int recordId = SaveRecord(KnowledgeRecords.NewRecord().MarkAsNew());
            Assert.That(recordId, Is.GreaterThan(1000));

            Assert.That(Records, Is.Not.Empty);

            SimpleKnowledgeModel model = Repository.FindById(recordId);
            Assert.That(model, Is.Not.Null);

            Assert.That(model.Location, Is.EqualTo(location));
            Assert.That(model.SamplePeriod, Is.GreaterThan(before).And.LessThan(after));
            Assert.That(model.Duration, Is.EqualTo(90));

        }

    }
}