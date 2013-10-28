using System;
using System.Collections.Generic;
using AmplaWeb.Data.Binding.MetaData;
using AmplaWeb.Data.Records;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.History
{
    [TestFixture]
    public class AmplaRecordHistoryUnitTests : TestFixture
    {
        private const string location = "Enterprise.Site.Area.Production";
        private const string module = "Production";

        [Test]
        public void SingleRecordWithNoHistory()
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
                    Module = record.Module,
                    Changes = new List<AmplaAuditSession>()
                };


            AmplaRecordHistory amplaRecordHistory = new AmplaRecordHistory(record, auditRecord);
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
            AmplaRecord record = new AmplaRecord(100)
            {
                Location = location,
                Module = module,
                ModelName = "Production Model"
            };
            record.AddColumn("CreatedDateTime", typeof(DateTime));
            record.AddColumn("CreatedBy", typeof(string));

            record.SetValue("CreatedBy", "Admin");
            DateTime created = DateTime.Today.AddHours(1);
            record.SetValue("CreatedDateTime", Iso8601DateTimeConverter.ConvertFromLocalDateTime(created));

            AmplaAuditRecord auditRecord = new AmplaAuditRecord
            {
                Id = record.Id,
                Location = record.Location,
                Module = record.Module,
                Changes = new List<AmplaAuditSession>()
            };
            
            AmplaRecordHistory amplaRecordHistory = new AmplaRecordHistory(record, auditRecord);
            List<AmplaRecordChanges> changes = amplaRecordHistory.Reconstruct();
            Assert.That(changes, Is.Not.Empty);
            Assert.That(changes.Count, Is.EqualTo(1));
            Assert.That(changes[0].Operation, Is.EqualTo("Create Record"));
            Assert.That(changes[0].User, Is.EqualTo("Admin"));
            Assert.That(changes[0].VersionDateTime, Is.EqualTo(created));
            Assert.That(changes[0].Changes, Is.Empty);
        }
    }
}