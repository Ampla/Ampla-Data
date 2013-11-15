using System.Collections.Generic;
using System.Linq;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.Modules.Production
{
    public class ProductionRecordUnitTests : TestFixture
    {
        private const string module = "Production";
        private const string location = "Enterprise.Site.Area.Production";

        [Test]
        public void NormalRecord()
        {
            InMemoryRecord record = ProductionRecords.NewRecord();
            Assert.That(record, Is.Not.Null);
            Assert.That(record.Module, Is.EqualTo(module));
            Assert.That(record.Location, Is.EqualTo(location));

            InMemoryRecord newRecord = ProductionRecords.NewRecord();
            Assert.That(record.RecordId, Is.Not.EqualTo(newRecord.RecordId));
        }

        [Test]
        public void DuplicateLocationBug()
        {
            InMemoryRecord record = ProductionRecords.NewRecord();

            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Fields.Count(field => field.Name == "Location"), Is.EqualTo(1));
            InMemoryRecord clone = record.Clone();

            Assert.That(clone.Location, Is.EqualTo(location));
            Assert.That(clone.Fields.Count(field => field.Name == "Location"), Is.EqualTo(1));
        }
    }
}