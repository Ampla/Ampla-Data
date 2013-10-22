using System;
using System.Collections.Generic;
using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Production;
using AmplaWeb.Data.Records;
using NUnit.Framework;

namespace AmplaWeb.Data.AmplaRepository
{
    [TestFixture]
    public class AmplaRepositoryUnitTests : AmplaRepositoryTestFixture<AmplaRepositoryUnitTests.AreaValueModel>
    {
        [AmplaLocation(Location = "Plant.Area.Values")]
        [AmplaModule(Module = "Production")]
        public class AreaValueModel
        {
            public int Id { get; set; }
            public double Value { get; set; }
            public string Area { get; set; }
        }

        private const string module = "Production";
        private const string location = "Plant.Area.Values";

        public AmplaRepositoryUnitTests() : base(module, location, ProductionViews.AreaValueModelView)
        {
        }

        [Test]
        public void GetAll()
        {
            IList<AreaValueModel> models = Repository.GetAll();
            Assert.That(models, Is.Empty);
        }

        [Test]
        public void GetAllWithExistingRecord()
        {
            InMemoryRecord record = ProductionRecords.NewRecord();
            record.Location = location;
            record.MarkAsNew();

            int recordId = SaveRecord(record);
            Assert.That(recordId, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            IList<AreaValueModel> models = Repository.GetAll();

            Assert.That(models, Is.Not.Empty);
            Assert.That(models[0].Id, Is.EqualTo(recordId));
        }

        [Test]
        public void FindById()
        {
            InMemoryRecord record = ProductionRecords.NewRecord();
            record.SetFieldValue("Value", 100);
            record.SetFieldValue("Area", "ROM");
            record.Location = location;
            record.MarkAsNew();

            int recordId = SaveRecord(record);
            Assert.That(recordId, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            AreaValueModel model = Repository.FindById(recordId);

            Assert.That(model.Id, Is.EqualTo(recordId));
            Assert.That(model.Value, Is.EqualTo(100));
            Assert.That(model.Area, Is.EqualTo("ROM"));

            Assert.That(Repository.FindById(recordId + 1), Is.Null);
        }


        [Test]
        public void FindByIdForDefaultModel()
        {
            InMemoryRecord record = ProductionRecords.NewRecord();
            //record.SetFieldValue("Value", 100);
            //record.SetFieldValue("Area", "ROM");
            record.Location = location;
            record.MarkAsNew();

            int recordId = SaveRecord(record);
            Assert.That(recordId, Is.GreaterThan(0));

            Assert.That(Records, Is.Not.Empty);

            AreaValueModel model = Repository.FindById(recordId);

            Assert.That(model.Id, Is.EqualTo(recordId));
            Assert.That(model.Value, Is.EqualTo(0));
            Assert.That(model.Area, Is.EqualTo(null));

            Assert.That(Repository.FindById(recordId + 1), Is.Null);
        }

        [Test]
        public void AddNewModel()
        {
            Assert.That(Records, Is.Empty);

            AreaValueModel model = new AreaValueModel {Area = "ROM", Value = 100};

            Repository.Add(model);
            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.GetFieldValue("Area",""), Is.EqualTo("ROM"));
            Assert.That(record.GetFieldValue<double>("Value", 0), Is.EqualTo(100.0d));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(record.Find("Location"), Is.Not.Null);

            Assert.That(model.Id, Is.EqualTo(record.RecordId));
        }

        /// <summary>
        /// Need to check that if an id is specified it is updated.
        /// </summary>
        [Test]
        public void AddNewModelWithExistingId()
        {
            Assert.That(Records, Is.Empty);

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100, Id = 99};

            Repository.Add(model);
            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.RecordId, Is.Not.EqualTo(99));
            Assert.That(record.GetFieldValue("Area", ""), Is.EqualTo("ROM"));
            Assert.That(record.GetFieldValue<double>("Value", 0), Is.EqualTo(100.0d));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.Not.EqualTo(DateTime.MinValue));

            Assert.That(model.Id, Is.EqualTo(record.RecordId));
        }

        [Test]
        public void DeleteModel()
        {
            Assert.That(Records, Is.Empty);

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            Repository.Add(model);
            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.IsDeleted(), Is.False);

            Assert.That(model.Id, Is.EqualTo(record.RecordId));

            AreaValueModel deleteModel = Repository.FindById(model.Id);
            Assert.That(deleteModel, Is.Not.Null);

            Repository.Delete(deleteModel);

            Assert.That(Records.Count, Is.EqualTo(1));

            record = Records[0];
            Assert.That(record.RecordId, Is.EqualTo(deleteModel.Id));
            Assert.That(record.IsDeleted(), Is.True);
        }

        [Test]
        public void ConfirmModel()
        {
            Assert.That(Records, Is.Empty);

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            Repository.Add(model);
            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.IsConfirmed(), Is.False);

            Assert.That(model.Id, Is.EqualTo(record.RecordId));

            AreaValueModel confirmModel = Repository.FindById(model.Id);
            Assert.That(confirmModel, Is.Not.Null);

            Repository.Confirm(confirmModel);

            Assert.That(Records.Count, Is.EqualTo(1));

            record = Records[0];
            Assert.That(record.RecordId, Is.EqualTo(confirmModel.Id));
            Assert.That(record.IsConfirmed(), Is.True);
        }

        [Test]
        public void UnconfirmModel()
        {
            Assert.That(Records, Is.Empty);

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            Repository.Add(model);
            Assert.That(Records, Is.Not.Empty);

            InMemoryRecord record = Records[0];
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.IsConfirmed(), Is.False);

            Assert.That(model.Id, Is.EqualTo(record.RecordId));

            AreaValueModel confirmModel = Repository.FindById(model.Id);
            Assert.That(confirmModel, Is.Not.Null);
            Repository.Confirm(confirmModel);

            Assert.That(Records.Count, Is.EqualTo(1));

            record = Records[0];
            Assert.That(record.RecordId, Is.EqualTo(confirmModel.Id));
            Assert.That(record.IsConfirmed(), Is.True);

            AreaValueModel unConfirmModel = Repository.FindById(model.Id);
            Assert.That(unConfirmModel, Is.Not.Null);
            Repository.Unconfirm(unConfirmModel);

            Assert.That(Records.Count, Is.EqualTo(1));

            record = Records[0];
            Assert.That(record.RecordId, Is.EqualTo(unConfirmModel.Id));
            Assert.That(record.IsConfirmed(), Is.False);
        }

        [Test]
        public void UpdateModel()
        {
            Assert.That(Records, Is.Empty);

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            Repository.Add(model);
            Assert.That(Records.Count, Is.EqualTo(1));

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.GetFieldValue("Area", ""), Is.EqualTo("ROM"));
            Assert.That(record.GetFieldValue<double>("Value", 0), Is.EqualTo(100.0d));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.Not.EqualTo(DateTime.MinValue));

            Assert.That(model.Id, Is.EqualTo(record.RecordId));

            AreaValueModel updateModel = Repository.FindById(model.Id);
            Assert.That(updateModel, Is.Not.Null);

            updateModel.Value = 200;
            Repository.Update(updateModel);

            Assert.That(Records.Count, Is.EqualTo(1));

            record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.RecordId, Is.EqualTo(model.Id));
            Assert.That(record.GetFieldValue("Area", ""), Is.EqualTo("ROM"));
            Assert.That(record.GetFieldValue<double>("Value", 0), Is.EqualTo(200.0d));
        }

        [Test]
        public void UpdateModelWithNoChanges()
        {
            Assert.That(Records, Is.Empty);

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            Repository.Add(model);
            Assert.That(Records.Count, Is.EqualTo(1));

            InMemoryRecord record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.GetFieldValue("Area", ""), Is.EqualTo("ROM"));
            Assert.That(record.GetFieldValue<double>("Value", 0), Is.EqualTo(100.0d));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.Not.EqualTo(DateTime.MinValue));

            Assert.That(model.Id, Is.EqualTo(record.RecordId));

            AreaValueModel updateModel = Repository.FindById(model.Id);
            Assert.That(updateModel, Is.Not.Null);

            int currentMessages = Messages.Count;
            
            Assert.That(currentMessages, Is.GreaterThan(0));

            updateModel.Value = 100;
            Repository.Update(updateModel);

            Assert.That(Messages.Count, Is.EqualTo(currentMessages + 1), string.Join("\r\n", Messages));
            // record is not submitted
            Assert.That(Records.Count, Is.EqualTo(1));

            record = Records[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.RecordId, Is.EqualTo(model.Id));
            Assert.That(record.GetFieldValue("Area", ""), Is.EqualTo("ROM"));
            Assert.That(record.GetFieldValue<double>("Value", 0), Is.EqualTo(100.0d));
        }

        [Test]
        public void FindRecord()
        {
            Assert.That(Records, Is.Empty);

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            Repository.Add(model);
            Assert.That(Records.Count, Is.EqualTo(1));

            AmplaRecord record = Repository.FindRecord(model.Id);
            Assert.That(record, Is.Not.Null);

            Assert.That(record.GetValue("Area"), Is.EqualTo(model.Area));
            Assert.That(record.GetValue("Value"), Is.EqualTo(model.Value));
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Id, Is.EqualTo(model.Id));
        }

        [Test]
        public void GetHistory_WrongId()
        {
            Assert.That(Records, Is.Empty);

            AmplaAuditRecord auditRecord = Repository.GetHistory(101);
            Assert.That(auditRecord, Is.Null);
        }

        [Test]
        public void GetHistory_NoChanges()
        {
            Assert.That(Records, Is.Empty);

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            Repository.Add(model);
            Assert.That(Records.Count, Is.EqualTo(1));

            AmplaRecord record = Repository.FindRecord(model.Id);
            Assert.That(record, Is.Not.Null);

            Assert.That(record.GetValue("Area"), Is.EqualTo(model.Area));
            Assert.That(record.GetValue("Value"), Is.EqualTo(model.Value));
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Id, Is.EqualTo(model.Id));

            AmplaAuditRecord auditRecord = Repository.GetHistory(model.Id);
            Assert.That(auditRecord, Is.Not.Null);

            Assert.That(auditRecord.Id, Is.EqualTo(model.Id));
            Assert.That(auditRecord.Location, Is.EqualTo(location));
            Assert.That(auditRecord.Module, Is.EqualTo(module));
            Assert.That(auditRecord.Changes, Is.Empty);
        }

        [Test]
        public void GetHistory_WithChanges()
        {
            Assert.That(Records, Is.Empty);

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            Repository.Add(model);
            Assert.That(Records.Count, Is.EqualTo(1));

            AmplaRecord record = Repository.FindRecord(model.Id);
            Assert.That(record, Is.Not.Null);

            Assert.That(record.GetValue("Area"), Is.EqualTo(model.Area));
            Assert.That(record.GetValue("Value"), Is.EqualTo(model.Value));
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Id, Is.EqualTo(model.Id));

            AreaValueModel updated = new AreaValueModel {Id = model.Id, Area = "ROM", Value = 200};

            DateTime before = DateTime.Now.AddMinutes(-1);
            DateTime after = DateTime.Now.AddMinutes(+1);

            Repository.Update(updated);
            record = Repository.FindRecord(model.Id);
            Assert.That(record, Is.Not.Null);

            Assert.That(record.GetValue("Area"), Is.EqualTo(model.Area));
            Assert.That(record.GetValue("Value"), Is.EqualTo(200));
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Id, Is.EqualTo(model.Id));

            AmplaAuditRecord auditRecord = Repository.GetHistory(model.Id);
            Assert.That(auditRecord, Is.Not.Null);

            Assert.That(auditRecord.Id, Is.EqualTo(model.Id));
            Assert.That(auditRecord.Location, Is.EqualTo(location));
            Assert.That(auditRecord.Module, Is.EqualTo(module));
            Assert.That(auditRecord.Changes, Is.Not.Empty);
            Assert.That(auditRecord.Changes.Count, Is.EqualTo(1));

            AmplaAuditSession session = auditRecord.Changes[0];
            Assert.That(session.User, Is.EqualTo("User"));
            Assert.That(session.EditedTime, Is.InRange(before, after));
            Assert.That(session.Fields, Is.Not.Empty);

            AssertAuditField(session, "Value", "100", "200");
           
        }
        
        [Test]
        public void GetVersions_NoChanges()
        {
            Assert.That(Records, Is.Empty);

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            Repository.Add(model);
            Assert.That(Records.Count, Is.EqualTo(1));

            AmplaRecord record = Repository.FindRecord(model.Id);
            Assert.That(record, Is.Not.Null);

            Assert.That(record.GetValue("Area"), Is.EqualTo(model.Area));
            Assert.That(record.GetValue("Value"), Is.EqualTo(model.Value));
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Id, Is.EqualTo(model.Id));

            IList<ModelVersion> versions = Repository.GetVersions(model.Id);
            Assert.That(versions, Is.Not.Empty);

            ModelVersion<AreaValueModel> version = (ModelVersion<AreaValueModel>) versions[0];

            Assert.That(version.Model, Is.Not.Null);
            Assert.That(version.Model.Id, Is.EqualTo(model.Id));
            Assert.That(version.Model.Value, Is.EqualTo(model.Value));
            Assert.That(version.Model.Area, Is.EqualTo(model.Area));

            Assert.That(versions[0].IsCurrentVersion, Is.True);
        }

        [Test]
        public void GetVersion_WithChanges()
        {
            Assert.That(Records, Is.Empty);

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            Repository.Add(model);
            Assert.That(Records.Count, Is.EqualTo(1));

            AmplaRecord record = Repository.FindRecord(model.Id);
            Assert.That(record, Is.Not.Null);

            AreaValueModel updated = new AreaValueModel { Id = model.Id, Area = "ROM", Value = 200 };

            Repository.Update(updated);
            record = Repository.FindRecord(model.Id);
            Assert.That(record, Is.Not.Null);

            Assert.That(record.GetValue("Area"), Is.EqualTo(model.Area));
            Assert.That(record.GetValue("Value"), Is.EqualTo(200));
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.Id, Is.EqualTo(model.Id));

            IList<ModelVersion> versions = Repository.GetVersions(model.Id);
            Assert.That(versions, Is.Not.Empty);
            Assert.That(versions.Count, Is.EqualTo(2));


            ModelVersion<AreaValueModel> last = (ModelVersion<AreaValueModel>) versions[0];
            ModelVersion<AreaValueModel> current = (ModelVersion<AreaValueModel>) versions[1];

            Assert.That(last.Model, Is.Not.Null);
            Assert.That(last.IsCurrentVersion, Is.False);
            Assert.That(last.Model.Id, Is.EqualTo(model.Id));
            Assert.That(last.Model.Value, Is.EqualTo(model.Value));
            Assert.That(last.Model.Area, Is.EqualTo(model.Area));

            Assert.That(current.Model, Is.Not.Null);
            Assert.That(current.IsCurrentVersion, Is.True);
            Assert.That(current.Model.Id, Is.EqualTo(updated.Id));
            Assert.That(current.Model.Value, Is.EqualTo(updated.Value));
            Assert.That(current.Model.Area, Is.EqualTo(updated.Area));
        }

        [Test]
        public void GetVersion_NotRelevantChanges()
        {
            Assert.That(Records, Is.Empty);

            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            Repository.Add(model);
            Assert.That(Records.Count, Is.EqualTo(1));

            int recordId = Records[0].RecordId;

            InMemoryRecord record = new InMemoryRecord {Module = module, Location = location, RecordId = recordId};
            record.SetFieldValue("Sample Period", DateTime.Today);

            UpdateRecord(record);

            Assert.That(Records.Count, Is.EqualTo(1));
            Assert.That(Records[0].GetFieldValue("Sample Period", DateTime.MinValue), Is.EqualTo(DateTime.Today.ToUniversalTime()));

            IList<ModelVersion> versions = Repository.GetVersions(model.Id);
            Assert.That(versions, Is.Not.Empty);
            Assert.That(versions.Count, Is.EqualTo(1));  // one version change ('Sample Period') is not relevant

            ModelVersion<AreaValueModel> current = (ModelVersion<AreaValueModel>) versions[0];

            Assert.That(current.Model, Is.Not.Null);
            Assert.That(current.IsCurrentVersion, Is.True);
            Assert.That(current.Model.Id, Is.EqualTo(model.Id));
            Assert.That(current.Model.Value, Is.EqualTo(model.Value));
            Assert.That(current.Model.Area, Is.EqualTo(model.Area));
        }

        [Test]
        public void GetVersions_WrongId()
        {
            Assert.That(Records, Is.Empty);

            IList<ModelVersion> versions = Repository.GetVersions(101);
            Assert.That(versions, Is.Empty);
        }
        
        private void AssertAuditField(AmplaAuditSession session, string field, string oldValue, string newValue)
        {
            AmplaAuditField value = new List<AmplaAuditField>(session.Fields).Find(f => f.Name == field);

            Assert.That(value, Is.Not.Null, "{0} not found", field);
            Assert.That(value.Name, Is.EqualTo(field), "Name is not set");
            Assert.That(value.OriginalValue, Is.EqualTo(oldValue), "{0} Original Value", field);
            Assert.That(value.EditedValue, Is.EqualTo(newValue), "{0} Edited Value", field);
        }
    }
}