using NUnit.Framework;

namespace AmplaWeb.Data.Records
{
    public class ProductionRecordUnitTests : TestFixture
    {
        private const string module = "Production";
        private const string location = "Plant.Area.Production";

        [Test]
        public void NormalRecord()
        {
            InMemoryRecord record = ProductionRecords.NewRecord();
            Assert.That(record, Is.Not.Null);
            Assert.That(record.Module, Is.EqualTo(module));
            Assert.That(record.Location, Is.EqualTo(location));

            InMemoryRecord newRecord = ProductionRecords.NewRecord();
            Assert.That(record.RecordId, Is.Not.EqualTo(newRecord.RecordId));
            Assert.That(record.GetFieldValue("Unique", ""), Is.Not.EqualTo(newRecord.GetFieldValue("Unique", "")));
        }


    }
}