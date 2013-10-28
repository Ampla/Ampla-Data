using System;
using System.Collections.Generic;
using AmplaWeb.Data.Records;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.History
{
    [TestFixture]
    public class ModifyRecordEventDectectionUnitTests : TestFixture
    {
        private const string location = "Enterprise.Site.Area.Production";
        private const string module = "Production";

        [Test]
        public void DetectChangesWithNoAuditTrail()
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


            ModifyRecordEventDectection recordEventDectection = new ModifyRecordEventDectection(record, auditRecord);
            List<AmplaRecordChanges> changes = recordEventDectection.DetectChanges();
            Assert.That(changes, Is.Empty);
        }

        [Test]
        public void DetectChangesWithOneFieldAuditTrail()
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
            auditRecord.Changes = new List<AmplaAuditSession>
                {
                    AddSession("User", DateTime.Today, "Value", "100", "200")
                };

            ModifyRecordEventDectection recordEventDectection = new ModifyRecordEventDectection(record, auditRecord);
            List<AmplaRecordChanges> changes = recordEventDectection.DetectChanges();
            Assert.That(changes, Is.Not.Empty);
            Assert.That(changes.Count, Is.EqualTo(1));

            Assert.That(changes[0].User, Is.EqualTo("User"));
            Assert.That(changes[0].VersionDateTime, Is.EqualTo(DateTime.Today));
            Assert.That(changes[0].Operation, Is.EqualTo("Modify Record"));
            Assert.That(changes[0].Changes, Is.Not.Empty);
            Assert.That(changes[0].Changes[0].Name, Is.EqualTo("Value"));
            Assert.That(changes[0].Changes[0].OriginalValue, Is.EqualTo("100"));
            Assert.That(changes[0].Changes[0].EditedValue, Is.EqualTo("200"));
        }

        [Test]
        public void DetectChangesWithMultipleFieldsAuditTrail()
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
                    Changes = new List<AmplaAuditSession>
                        {
                            AddSession("User", DateTime.Today, "Value", "100", "200"),
                            AddSession("Admin", DateTime.Today.AddHours(1), 
                                        new[] {"One", "Two"}, 
                                        new[] {"11", "22"},
                                        new[] {"111", "222"})
                        }
                };

            ModifyRecordEventDectection recordEventDectection = new ModifyRecordEventDectection(record, auditRecord);
            List<AmplaRecordChanges> changes = recordEventDectection.DetectChanges();
            Assert.That(changes, Is.Not.Empty);
            Assert.That(changes.Count, Is.EqualTo(2));

            Assert.That(changes[0].User, Is.EqualTo("User"));
            Assert.That(changes[0].VersionDateTime, Is.EqualTo(DateTime.Today));
            Assert.That(changes[0].Operation, Is.EqualTo("Modify Record"));
            Assert.That(changes[0].Changes, Is.Not.Empty);
            Assert.That(changes[0].Changes[0].Name, Is.EqualTo("Value"));
            Assert.That(changes[0].Changes[0].OriginalValue, Is.EqualTo("100"));
            Assert.That(changes[0].Changes[0].EditedValue, Is.EqualTo("200"));

            Assert.That(changes[1].User, Is.EqualTo("Admin"));
            Assert.That(changes[1].VersionDateTime, Is.EqualTo(DateTime.Today.AddHours(1)));
            Assert.That(changes[1].Operation, Is.EqualTo("Modify Record"));
            Assert.That(changes[1].Changes, Is.Not.Empty);
            Assert.That(changes[1].Changes.Length, Is.EqualTo(2));

            Assert.That(changes[1].Changes[0].Name, Is.EqualTo("One"));
            Assert.That(changes[1].Changes[0].OriginalValue, Is.EqualTo("11"));
            Assert.That(changes[1].Changes[0].EditedValue, Is.EqualTo("111"));
            
            Assert.That(changes[1].Changes[1].Name, Is.EqualTo("Two"));
            Assert.That(changes[1].Changes[1].OriginalValue, Is.EqualTo("22"));
            Assert.That(changes[1].Changes[1].EditedValue, Is.EqualTo("222"));
        }

        [Test]
        public void DetectChangesWithAuditTrailWithNoChangeInValue()
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
            auditRecord.Changes = new List<AmplaAuditSession>
                {
                    AddSession("User", DateTime.Today, "Value", "100", "100")
                };

            ModifyRecordEventDectection recordEventDectection = new ModifyRecordEventDectection(record, auditRecord);
            List<AmplaRecordChanges> changes = recordEventDectection.DetectChanges();
            Assert.That(changes, Is.Empty);
        }

        [Test]
        public void DetectChangesWithAuditTrailWithNoChangesInValues()
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
            auditRecord.Changes = new List<AmplaAuditSession>
                {
                    AddSession("User", DateTime.Today, "Value", "100", "100"),
                    AddSession("Admin", DateTime.Today.AddHours(1), 
                                        new[] {"One", "Two"}, 
                                        new[] {"11", "222"},
                                        new[] {"111", "222"})
                };

            ModifyRecordEventDectection recordEventDectection = new ModifyRecordEventDectection(record, auditRecord);
            List<AmplaRecordChanges> changes = recordEventDectection.DetectChanges();
            Assert.That(changes, Is.Not.Empty);
            Assert.That(changes.Count, Is.EqualTo(1));

            Assert.That(changes[0].User, Is.EqualTo("Admin"));
            Assert.That(changes[0].VersionDateTime, Is.EqualTo(DateTime.Today.AddHours(1)));
            Assert.That(changes[0].Operation, Is.EqualTo("Modify Record"));
            Assert.That(changes[0].Changes, Is.Not.Empty);
            Assert.That(changes[0].Changes.Length, Is.EqualTo(1));
            Assert.That(changes[0].Changes[0].Name, Is.EqualTo("One"));
            Assert.That(changes[0].Changes[0].OriginalValue, Is.EqualTo("11"));
            Assert.That(changes[0].Changes[0].EditedValue, Is.EqualTo("111"));
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
            return AddSession(user, time, new [] {field},new [] {oldValue}, new[] {newValue} );
        }
    }


}