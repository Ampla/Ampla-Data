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
            record.AddColumn("CreatedDateTime", typeof (DateTime));
            record.AddColumn("CreatedBy", typeof (string));

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

        [Test]
        public void SingleRecordThatHasBeenDeleted()
        {
            DateTime createdTime = DateTime.Today.AddDays(-1);
            AmplaRecord record = new AmplaRecord(100)
                {
                    Location = location,
                    Module = module,
                    ModelName = "Production Model"
                };
            record.AddColumn("CreatedDateTime", typeof(DateTime));
            record.AddColumn("Deleted", typeof(bool));

            record.SetValue("CreatedDateTime", Iso8601DateTimeConverter.ConvertFromLocalDateTime(createdTime));
            record.SetValue("Deleted", "True");

            AmplaAuditRecord auditRecord = new AmplaAuditRecord
                {
                    Id = record.Id,
                    Location = record.Location,
                    Module = record.Module,
                    Changes = new List<AmplaAuditSession>
                        {
                            AddSession("User", DateTime.Today, "IsDeleted", "False", "True")
                        }
                };

            AmplaRecordHistory amplaRecordHistory = new AmplaRecordHistory(record, auditRecord);
            List<AmplaRecordChanges> changes = amplaRecordHistory.Reconstruct();
            Assert.That(changes, Is.Not.Empty);

            Assert.That(changes, Is.Not.Empty);
            Assert.That(changes.Count, Is.EqualTo(2));
            Assert.That(changes[0].Operation, Is.EqualTo("Create Record"));
            Assert.That(changes[0].User, Is.EqualTo("System"));
            Assert.That(changes[0].Changes, Is.Empty);
            Assert.That(changes[1].Operation, Is.EqualTo("Delete Record"));
            Assert.That(changes[1].User, Is.EqualTo("User"));
            Assert.That(changes[1].Changes, Is.Empty);
        }

        private static AmplaAuditSession AddSession(string user, DateTime time, string[] fields, string[] oldValues,
                                     string[] newValues)
        {
            AmplaAuditSession session = new AmplaAuditSession(user, time);

            Assert.That(fields.Length, Is.GreaterThan(0));
            Assert.That(fields.Length, Is.EqualTo(oldValues.Length));
            Assert.That(fields.Length, Is.EqualTo(newValues.Length));
            for (int i = 0; i < fields.Length; i++)
            {
                AmplaAuditField field = new AmplaAuditField
                {
                    Name = fields[i],
                    OriginalValue = oldValues[i],
                    EditedValue = newValues[i]
                };
                session.Fields.Add(field);
            }
            return session;
        }

        private static AmplaAuditSession AddSession(string user, DateTime time, string field, string oldValue,
                                             string newValue)
        {
            return AddSession(user, time, new[] { field }, new[] { oldValue }, new[] { newValue });
        }

    }
}