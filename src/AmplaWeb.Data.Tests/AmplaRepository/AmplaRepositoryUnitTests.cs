using System;
using System.Collections.Generic;
using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Records;
using AmplaWeb.Data.Views;
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
    }
}