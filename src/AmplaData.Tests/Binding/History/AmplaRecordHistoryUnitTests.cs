using System;
using System.Collections.Generic;
using AmplaData.Binding.MetaData;
using AmplaData.Binding.ViewData;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.Binding.History
{
    [TestFixture]
    public class AmplaRecordHistoryUnitTests : RecordEventDetectionTestFixture
    {
        [Test]
        public void SingleRecordWithNoHistory()
        {
            AmplaRecord record = CreateRecord(100);
            AmplaAuditRecord auditRecord = CreateAuditRecord(record);
            IAmplaViewProperties<ProductionModel> viewProperties = GetViewProperties();

            AmplaRecordHistory<ProductionModel> amplaRecordHistory = new AmplaRecordHistory<ProductionModel>(record, auditRecord, viewProperties);
            List<AmplaRecordChanges> changes = amplaRecordHistory.Reconstruct();
            Assert.That(changes, Is.Not.Empty);

            Assert.That(changes, Is.Not.Empty);
            Assert.That(changes.Count, Is.EqualTo(1));
            Assert.That(changes[0].Operation, Is.EqualTo("Create Record"));
            Assert.That(changes[0].User, Is.EqualTo("System"));
            Assert.That(changes[0].Changes, Is.Empty);
        }

        [Test]
        public void SingleRecordWithCreateHistory()
        {
            AmplaRecord record = CreateRecord(100);
            AmplaAuditRecord auditRecord = CreateAuditRecord(record);
            IAmplaViewProperties<ProductionModel> viewProperties = GetViewProperties();
            record.AddColumn("CreatedDateTime", typeof(DateTime));
            record.AddColumn("CreatedBy", typeof (string));

            record.SetValue("CreatedBy", "Admin");
            DateTime created = DateTime.Today.AddHours(1);
            record.SetValue("CreatedDateTime", Iso8601DateTimeConverter.ConvertFromLocalDateTime(created));

            AmplaRecordHistory<ProductionModel> amplaRecordHistory = new AmplaRecordHistory<ProductionModel>(record, auditRecord, viewProperties);
            List<AmplaRecordChanges> changes = amplaRecordHistory.Reconstruct();
            Assert.That(changes, Is.Not.Empty);
            Assert.That(changes.Count, Is.EqualTo(1));
            Assert.That(changes[0].Operation, Is.EqualTo("Create Record"));
            Assert.That(changes[0].User, Is.EqualTo("Admin"));
            Assert.That(changes[0].VersionDateTime, Is.EqualTo(created));
            Assert.That(changes[0].Changes, Is.Empty);
        }

        [Test]
        public void SingleRecordThatHasBeenDeleted()
        {
            DateTime createdTime = DateTime.Today.AddDays(-1);
            AmplaRecord record = CreateRecord(100);
            AmplaAuditRecord auditRecord = CreateAuditRecord(record);
            IAmplaViewProperties<ProductionModel> viewProperties = GetViewProperties();
            record.AddColumn("CreatedDateTime", typeof(DateTime));
            record.AddColumn("Deleted", typeof(bool));

            record.SetValue("CreatedDateTime", Iso8601DateTimeConverter.ConvertFromLocalDateTime(createdTime));
            record.SetValue("Deleted", "True");
            auditRecord.Changes.Add
                (
                    AddSession("User", DateTime.Today, "IsDeleted", "False", "True")
                );
       
            AmplaRecordHistory<ProductionModel> amplaRecordHistory = new AmplaRecordHistory<ProductionModel>(record, auditRecord, viewProperties);
            List<AmplaRecordChanges> changes = amplaRecordHistory.Reconstruct();
            Assert.That(changes, Is.Not.Empty);
            Assert.That(changes.Count, Is.EqualTo(2));
            Assert.That(changes[0].Operation, Is.EqualTo("Create Record"));
            Assert.That(changes[0].User, Is.EqualTo("System"));
            Assert.That(changes[0].Changes, Is.Empty);
            Assert.That(changes[1].Operation, Is.EqualTo("Delete Record"));
            Assert.That(changes[1].User, Is.EqualTo("User"));
            Assert.That(changes[1].Changes, Is.Not.Empty);
            AmplaAuditField deleted = changes[1].Changes[0];
            Assert.That(deleted.Name, Is.EqualTo("IsDeleted"));
        }

    }
}