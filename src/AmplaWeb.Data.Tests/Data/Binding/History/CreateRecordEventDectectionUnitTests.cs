using System;
using System.Collections.Generic;
using AmplaData.Data.Binding.MetaData;
using AmplaData.Data.Binding.ViewData;
using AmplaData.Data.Records;
using NUnit.Framework;

namespace AmplaData.Data.Binding.History
{
    [TestFixture]
    public class CreateRecordEventDectectionUnitTests : RecordEventDetectionTestFixture
    {
        [Test]
        public void GetRecordCreatedEvent()
        {
            AmplaRecord record = CreateRecord(100);
            AmplaAuditRecord auditRecord = CreateAuditRecord(record);
            IAmplaViewProperties<ProductionModel> viewProperties = GetViewProperties();

            record.AddColumn("CreatedDateTime", typeof(DateTime));
            record.AddColumn("CreatedBy", typeof(string));

            record.SetValue("CreatedBy", "User");
            DateTime created = DateTime.Today.AddHours(1);
            record.SetValue("CreatedDateTime", Iso8601DateTimeConverter.ConvertFromLocalDateTime(created));

            CreateRecordEventDectection<ProductionModel> recordEventDectection = new CreateRecordEventDectection<ProductionModel>(record, auditRecord, viewProperties);
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
            AmplaRecord record = CreateRecord(100);
            AmplaAuditRecord auditRecord = CreateAuditRecord(record);
            IAmplaViewProperties<ProductionModel> viewProperties = GetViewProperties();
            record.AddColumn("CreatedDateTime", typeof(DateTime));
            record.AddColumn("CreatedBy", typeof(string));

            //record.SetValue("CreatedBy", "User");
            DateTime created = DateTime.Today.AddHours(1);
            record.SetValue("CreatedDateTime", Iso8601DateTimeConverter.ConvertFromLocalDateTime(created));

            CreateRecordEventDectection<ProductionModel> recordEventDectection = new CreateRecordEventDectection<ProductionModel>(record, auditRecord, viewProperties);
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
            AmplaRecord record = CreateRecord(100);
            AmplaAuditRecord auditRecord = CreateAuditRecord(record);
            IAmplaViewProperties<ProductionModel> viewProperties = GetViewProperties();
            record.AddColumn("CreatedDateTime", typeof(DateTime));
            record.AddColumn("CreatedBy", typeof(string));

            record.SetValue("CreatedBy", "System Configuration.Users.User");
            DateTime created = DateTime.Today.AddHours(1);
            record.SetValue("CreatedDateTime", Iso8601DateTimeConverter.ConvertFromLocalDateTime(created));

            CreateRecordEventDectection<ProductionModel> recordEventDectection = new CreateRecordEventDectection<ProductionModel>(record, auditRecord, viewProperties);
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