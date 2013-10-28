using System;
using System.Collections.Generic;
using AmplaWeb.Data.Binding.MetaData;
using AmplaWeb.Data.Records;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.History
{
    [TestFixture]
    public class CreateRecordEventDectectionUnitTests : TestFixture
    {
        private const string location = "Enterprise.Site.Area.Production";
        private const string module = "Production";

        [Test]
        public void GetRecordCreatedEvent()
        {
            AmplaRecord record = new AmplaRecord(100)
                {
                    Location = location,
                    Module = module,
                    ModelName = "Production Model"
                };

            AmplaAuditRecord auditRecord = new AmplaAuditRecord
            {
                Id = record.Id,
                Location = record.Location,
                Module = record.Module
            };
            record.AddColumn("CreatedDateTime", typeof(DateTime));
            record.AddColumn("CreatedBy", typeof(string));

            record.SetValue("CreatedBy", "User");
            DateTime created = DateTime.Today.AddHours(1);
            record.SetValue("CreatedDateTime", Iso8601DateTimeConverter.ConvertFromLocalDateTime(created));

            CreateRecordEventDectection recordEventDectection = new CreateRecordEventDectection(record, auditRecord);
            List<AmplaRecordChanges> changes = recordEventDectection.DetectChanges();
            Assert.That(changes, Is.Not.Empty);
            Assert.That(changes[0].Operation, Is.EqualTo("Create Record"));
            Assert.That(changes[0].User, Is.EqualTo("User"));
            Assert.That(changes[0].VersionDateTime, Is.EqualTo(created));
            Assert.That(changes[0].Changes, Is.Empty);
            Assert.That(changes[0].Display, Is.EqualTo("User created record"));
        }

        [Test]
        public void GetRecordCreatedEventSystemRecord()
        {
            AmplaRecord record = new AmplaRecord(100)
            {
                Location = location,
                Module = module,
                ModelName = "Production Model"
            };

            AmplaAuditRecord auditRecord = new AmplaAuditRecord
            {
                Id = record.Id,
                Location = record.Location,
                Module = record.Module
            };
            record.AddColumn("CreatedDateTime", typeof(DateTime));
            record.AddColumn("CreatedBy", typeof(string));

            //record.SetValue("CreatedBy", "User");
            DateTime created = DateTime.Today.AddHours(1);
            record.SetValue("CreatedDateTime", Iso8601DateTimeConverter.ConvertFromLocalDateTime(created));

            CreateRecordEventDectection recordEventDectection = new CreateRecordEventDectection(record, auditRecord);
            List<AmplaRecordChanges> changes = recordEventDectection.DetectChanges();
            Assert.That(changes, Is.Not.Empty);
            Assert.That(changes[0].Operation, Is.EqualTo("Create Record"));
            Assert.That(changes[0].User, Is.EqualTo("System"));
            Assert.That(changes[0].VersionDateTime, Is.EqualTo(created));
            Assert.That(changes[0].Changes, Is.Empty);
            Assert.That(changes[0].Display, Is.EqualTo("System created record"));
        }

        [Test]
        public void ShowSimpleUsersName()
        {
            AmplaRecord record = new AmplaRecord(100)
            {
                Location = location,
                Module = module,
                ModelName = "Production Model"
            };

            AmplaAuditRecord auditRecord = new AmplaAuditRecord
            {
                Id = record.Id,
                Location = record.Location,
                Module = record.Module
            };
            record.AddColumn("CreatedDateTime", typeof(DateTime));
            record.AddColumn("CreatedBy", typeof(string));

            record.SetValue("CreatedBy", "System Configuration.Users.User");
            DateTime created = DateTime.Today.AddHours(1);
            record.SetValue("CreatedDateTime", Iso8601DateTimeConverter.ConvertFromLocalDateTime(created));

            CreateRecordEventDectection recordEventDectection = new CreateRecordEventDectection(record, auditRecord);
            List<AmplaRecordChanges> changes = recordEventDectection.DetectChanges();
            Assert.That(changes, Is.Not.Empty);
            Assert.That(changes[0].Operation, Is.EqualTo("Create Record"));
            Assert.That(changes[0].User, Is.EqualTo("User"));
            Assert.That(changes[0].VersionDateTime, Is.EqualTo(created));
            Assert.That(changes[0].Changes, Is.Empty);
            Assert.That(changes[0].Display, Is.EqualTo("User created record"));
        }
    }
}