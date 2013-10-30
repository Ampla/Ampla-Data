using System;
using System.Collections.Generic;
using AmplaWeb.Data.Binding.MetaData;
using AmplaWeb.Data.Binding.ViewData;
using AmplaWeb.Data.Records;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.History
{
    [TestFixture]
    public class ModifyRecordEventDectectionUnitTests : RecordEventDetectionTestFixture
    {

        [Test]
        public void DetectChangesWithNoAuditTrail()
        {
            AmplaRecord record = CreateRecord(100);
            AmplaAuditRecord auditRecord = CreateAuditRecord(record);
            IAmplaViewProperties<ProductionModel> viewProperties = GetViewProperties();

            ModifyRecordEventDectection<ProductionModel> recordEventDectection = new ModifyRecordEventDectection<ProductionModel>(record, auditRecord, viewProperties);
            List<AmplaRecordChanges> changes = recordEventDectection.DetectChanges();
            Assert.That(changes, Is.Empty);
        }

        [Test]
        public void DetectChangesWithOneFieldAuditTrail()
        {
            AmplaRecord record = CreateRecord(100);
            AmplaAuditRecord auditRecord = CreateAuditRecord(record);
            IAmplaViewProperties<ProductionModel> viewProperties = GetViewProperties();

            auditRecord.Changes = new List<AmplaAuditSession>
                {
                    AddSession("User", DateTime.Today, "Value", "100", "200")
                };

            ModifyRecordEventDectection<ProductionModel> recordEventDectection = new ModifyRecordEventDectection<ProductionModel>(record, auditRecord, viewProperties);
            
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
            AmplaRecord record = CreateRecord(100);
            AmplaAuditRecord auditRecord = CreateAuditRecord(record);
            IAmplaViewProperties<ProductionModel> viewProperties = GetViewProperties();

            auditRecord.Changes = new List<AmplaAuditSession>
                {
                            AddSession("User", DateTime.Today, "Value", "100", "200"),
                            AddSession("Admin", DateTime.Today.AddHours(1), 
                                        new[] {"One", "Two"}, 
                                        new[] {"11", "22"},
                                        new[] {"111", "222"})
                };

            ModifyRecordEventDectection<ProductionModel> recordEventDectection = new ModifyRecordEventDectection<ProductionModel>(record, auditRecord, viewProperties);
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
            AmplaRecord record = CreateRecord(100);
            AmplaAuditRecord auditRecord = CreateAuditRecord(record);
            IAmplaViewProperties<ProductionModel> viewProperties = GetViewProperties();

            auditRecord.Changes = new List<AmplaAuditSession>
                {
                    AddSession("User", DateTime.Today, "Value", "100", "100")
                };

            ModifyRecordEventDectection<ProductionModel> recordEventDectection = new ModifyRecordEventDectection<ProductionModel>(record, auditRecord, viewProperties);
            List<AmplaRecordChanges> changes = recordEventDectection.DetectChanges();
            Assert.That(changes, Is.Empty);
        }

        [Test]
        public void DetectChangesWithAuditTrailWithNoChangesInValues()
        {
            AmplaRecord record = CreateRecord(100);
            AmplaAuditRecord auditRecord = CreateAuditRecord(record);
            IAmplaViewProperties<ProductionModel> viewProperties = GetViewProperties();
            auditRecord.Changes = new List<AmplaAuditSession>
                {
                    AddSession("User", DateTime.Today, "Value", "100", "100"),
                    AddSession("Admin", DateTime.Today.AddHours(1), 
                                        new[] {"One", "Two"}, 
                                        new[] {"11", "222"},
                                        new[] {"111", "222"})
                };

            ModifyRecordEventDectection<ProductionModel> recordEventDectection = new ModifyRecordEventDectection<ProductionModel>(record, auditRecord, viewProperties);
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

        [Test]
        public void DetectChangesWithDeletedRecords()
        {
            AmplaRecord record = CreateRecord(100);
            AmplaAuditRecord auditRecord = CreateAuditRecord(record);
            IAmplaViewProperties<ProductionModel> viewProperties = GetViewProperties();
            record.AddColumn("Sample Period", typeof(DateTime));
            record.SetValue("Sample Period", Iso8601DateTimeConverter.ConvertFromLocalDateTime(DateTime.Today.AddDays(-1)));
            auditRecord.Changes = new List<AmplaAuditSession>
                {
                    AddSession("User", DateTime.Today, "IsDeleted", "False", "True")
                };

            ModifyRecordEventDectection<ProductionModel> recordEventDectection = new ModifyRecordEventDectection<ProductionModel>(record, auditRecord, viewProperties);
            List<AmplaRecordChanges> changes = recordEventDectection.DetectChanges();
            Assert.That(changes, Is.Empty);
        }

 
    }


}