using System;
using NUnit.Framework;

namespace AmplaWeb.Data.Records
{
    [TestFixture]
    public class AmplaRecordUnitTests : TestFixture
    {
        [Test]
        public void Id()
        {
            AmplaRecord record = new AmplaRecord(100);
            Assert.That(record.Id, Is.EqualTo(100));

            record.Id = 200;

            Assert.That(record.Id, Is.EqualTo(200));
        }

        [Test]
        public void Location()
        {
            AmplaRecord record = new AmplaRecord(100);
            Assert.That(record.Location, Is.Null);

            record.Location = "Plant.Area.Point";

            Assert.That(record.Location, Is.EqualTo("Plant.Area.Point"));

            record.SetValue("Location", "Plant.Area.Equipment.Point");
        }

        [Test]
        public void LocationPropertyIsSetViaSetValue()
        {
            AmplaRecord record = new AmplaRecord(100);

            Assert.That(record.Location, Is.Null);
            record.AddColumn("Location", typeof(string));

            Assert.That(record.Location, Is.Null);

            record.SetValue("Location", "Plant.Area.Point");
            Assert.That(record.Location, Is.EqualTo("Plant.Area.Point"));
        }


        [Test]
        public void LocationGetValueRetrievesLocationField()
        {
            AmplaRecord record = new AmplaRecord(100) {Location = "Plant.Area.Point"};

            Assert.That(record.GetValue("Location"), Is.EqualTo("Plant.Area.Point"));
            Assert.That(record.Location, Is.EqualTo("Plant.Area.Point"));
        }

        [Test]
        public void GetFieldNames_NoFields()
        {
            AmplaRecord record = new AmplaRecord(100);

            Assert.That(record.GetFieldNames(), Is.Not.Empty);

            Assert.That(record.GetValue("Location"), Is.Null);

            Assert.That(record.Location, Is.Null);

            record.SetValue("Location", "Plant.Area.Point");
            Assert.That(record.Location, Is.EqualTo("Plant.Area.Point"));
        }

        [Test]
        public void GetValue_Location()
        {
            AmplaRecord record = new AmplaRecord(100) { Location = "Plant.Area.Point" };

            Assert.That(record.GetValue("Location"), Is.EqualTo("Plant.Area.Point"));
        }

        [Test]
        public void AddColumn()
        {
            AmplaRecord record = new AmplaRecord(100);
            record.AddColumn("Field", typeof(int));
            Assert.That(record.GetValue("Field"), Is.Null);
        }

        [Test]
        public void AddColumnSameType()
        {
            AmplaRecord record = new AmplaRecord(100);
            record.AddColumn("Location", typeof(string));
            record.AddColumn("Location", typeof(string));

            Assert.That(record.GetFieldNames().Length, Is.EqualTo(1));
            Assert.That(record.GetFieldNames()[0], Is.EqualTo("Location"));

            Assert.That(record.GetValue("Location"), Is.Null);
        }

        [Test]
        public void AddColumnDifferentType()
        {
            AmplaRecord record = new AmplaRecord(100);
            record.AddColumn("Field", typeof(string));

            Assert.Throws<ArgumentException>(() => record.AddColumn("Field", typeof (int)));
        }
    }
}