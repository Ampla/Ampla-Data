using System;
using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Logging;
using AmplaWeb.Data.Records;
using AmplaWeb.Data.Tests;
using AmplaWeb.Data.Views;
using NUnit.Framework;

namespace AmplaWeb.Data.AmplaRepository
{
    [TestFixture]
    public class AmplaRepositoryUnitTests : TestFixture
    {
        [AmplaLocation(Location = "Plant.Area.Values")]
        [AmplaModule(Module = "Production")]
        public class AreaValueModel
        {
            public int Id { get; set; }
            public double Value { get; set; }
            public string Area { get; set; }
        }

        private const string userName = "User";
        private const string password = "password";
        private const string module = "Production";
        private const string location = "Plant.Area.Values";

        [Test]
        public void GetAll()
        {
            IDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);
            AmplaReadOnlyRepository<AreaValueModel> repository = new AmplaReadOnlyRepository<AreaValueModel>(webServiceClient, userName, password);
            IList<AreaValueModel> models = repository.GetAll();

            Assert.That(models, Is.Empty);
        }

        [Test]
        public void GetAllWithExistingRecord()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);
            InMemoryRecord record = ProductionRecords.NewRecord();
            record.Location = location;
            record.MarkAsNew();

            int recordId = record.SaveTo(webServiceClient);
            Assert.That(recordId, Is.GreaterThan(0));

            Assert.That(webServiceClient.DatabaseRecords, Is.Not.Empty);

            AmplaReadOnlyRepository<AreaValueModel> repository = new AmplaReadOnlyRepository<AreaValueModel>(webServiceClient, userName, password);
            IList<AreaValueModel> models = repository.GetAll();

            Assert.That(models, Is.Not.Empty);
            Assert.That(models[0].Id, Is.EqualTo(recordId));
        }

        [Test]
        public void FindById()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);
            InMemoryRecord record = ProductionRecords.NewRecord();
            record.SetFieldValue("Value", 100);
            record.SetFieldValue("Area", "ROM");
            record.Location = location;
            record.MarkAsNew();

            int recordId = record.SaveTo(webServiceClient);
            Assert.That(recordId, Is.GreaterThan(0));

            Assert.That(webServiceClient.DatabaseRecords, Is.Not.Empty);

            AmplaReadOnlyRepository<AreaValueModel> repository = new AmplaReadOnlyRepository<AreaValueModel>(webServiceClient, userName, password);
            AreaValueModel model = repository.FindById(recordId);

            Assert.That(model.Id, Is.EqualTo(recordId));
            Assert.That(model.Value, Is.EqualTo(100));
            Assert.That(model.Area, Is.EqualTo("ROM"));

            Assert.That(repository.FindById(recordId + 1), Is.Null);
        }


        [Test]
        public void FindByIdForDefaultModel()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);
            InMemoryRecord record = ProductionRecords.NewRecord();
            //record.SetFieldValue("Value", 100);
            //record.SetFieldValue("Area", "ROM");
            record.Location = location;
            record.MarkAsNew();

            int recordId = record.SaveTo(webServiceClient);
            Assert.That(recordId, Is.GreaterThan(0));

            Assert.That(webServiceClient.DatabaseRecords, Is.Not.Empty);

            AmplaReadOnlyRepository<AreaValueModel> repository = new AmplaReadOnlyRepository<AreaValueModel>(webServiceClient, userName, password);
            AreaValueModel model = repository.FindById(recordId);

            Assert.That(model.Id, Is.EqualTo(recordId));
            Assert.That(model.Value, Is.EqualTo(0));
            Assert.That(model.Area, Is.EqualTo(null));

            Assert.That(repository.FindById(recordId + 1), Is.Null);
        }

        [Test]
        public void AddNewModel()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);
            webServiceClient.GetViewFunc = ProductionViews.AreaValueModelView;
            Assert.That(webServiceClient.DatabaseRecords, Is.Empty);

            AmplaRepository<AreaValueModel> repository = new AmplaRepository<AreaValueModel>(webServiceClient, userName, password);
            AreaValueModel model = new AreaValueModel {Area = "ROM", Value = 100};

            repository.Add(model);
            Assert.That(webServiceClient.DatabaseRecords, Is.Not.Empty);

            InMemoryRecord record = webServiceClient.DatabaseRecords[0];
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
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);
            webServiceClient.GetViewFunc = ProductionViews.AreaValueModelView;
            Assert.That(webServiceClient.DatabaseRecords, Is.Empty);

            AmplaRepository<AreaValueModel> repository = new AmplaRepository<AreaValueModel>(webServiceClient, userName, password);
            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100, Id = 99};

            repository.Add(model);
            Assert.That(webServiceClient.DatabaseRecords, Is.Not.Empty);

            InMemoryRecord record = webServiceClient.DatabaseRecords[0];
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
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);
            webServiceClient.GetViewFunc = ProductionViews.AreaValueModelView;
            Assert.That(webServiceClient.DatabaseRecords, Is.Empty);

            AmplaRepository<AreaValueModel> repository = new AmplaRepository<AreaValueModel>(webServiceClient, userName, password);
            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            repository.Add(model);
            Assert.That(webServiceClient.DatabaseRecords, Is.Not.Empty);

            InMemoryRecord record = webServiceClient.DatabaseRecords[0];
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.IsDeleted(), Is.False);

            Assert.That(model.Id, Is.EqualTo(record.RecordId));

            AreaValueModel deleteModel = repository.FindById(model.Id);
            Assert.That(deleteModel, Is.Not.Null);

            repository.Delete(deleteModel);

            Assert.That(webServiceClient.DatabaseRecords.Count, Is.EqualTo(1));

            record = webServiceClient.DatabaseRecords[0];
            Assert.That(record.RecordId, Is.EqualTo(deleteModel.Id));
            Assert.That(record.IsDeleted(), Is.True);
        }

        [Test]
        public void ConfirmModel()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);
            webServiceClient.GetViewFunc = ProductionViews.AreaValueModelView;
            Assert.That(webServiceClient.DatabaseRecords, Is.Empty);

            AmplaRepository<AreaValueModel> repository = new AmplaRepository<AreaValueModel>(webServiceClient, userName, password);
            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            repository.Add(model);
            Assert.That(webServiceClient.DatabaseRecords, Is.Not.Empty);

            InMemoryRecord record = webServiceClient.DatabaseRecords[0];
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.IsConfirmed(), Is.False);

            Assert.That(model.Id, Is.EqualTo(record.RecordId));

            AreaValueModel confirmModel = repository.FindById(model.Id);
            Assert.That(confirmModel, Is.Not.Null);

            repository.Confirm(confirmModel);

            Assert.That(webServiceClient.DatabaseRecords.Count, Is.EqualTo(1));

            record = webServiceClient.DatabaseRecords[0];
            Assert.That(record.RecordId, Is.EqualTo(confirmModel.Id));
            Assert.That(record.IsConfirmed(), Is.True);
        }

        [Test]
        public void UnconfirmModel()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);
            webServiceClient.GetViewFunc = ProductionViews.AreaValueModelView;
            Assert.That(webServiceClient.DatabaseRecords, Is.Empty);

            AmplaRepository<AreaValueModel> repository = new AmplaRepository<AreaValueModel>(webServiceClient, userName, password);
            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            repository.Add(model);
            Assert.That(webServiceClient.DatabaseRecords, Is.Not.Empty);

            InMemoryRecord record = webServiceClient.DatabaseRecords[0];
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.IsConfirmed(), Is.False);

            Assert.That(model.Id, Is.EqualTo(record.RecordId));

            AreaValueModel confirmModel = repository.FindById(model.Id);
            Assert.That(confirmModel, Is.Not.Null);
            repository.Confirm(confirmModel);

            Assert.That(webServiceClient.DatabaseRecords.Count, Is.EqualTo(1));

            record = webServiceClient.DatabaseRecords[0];
            Assert.That(record.RecordId, Is.EqualTo(confirmModel.Id));
            Assert.That(record.IsConfirmed(), Is.True);

            AreaValueModel unConfirmModel = repository.FindById(model.Id);
            Assert.That(unConfirmModel, Is.Not.Null);
            repository.Unconfirm(unConfirmModel);

            Assert.That(webServiceClient.DatabaseRecords.Count, Is.EqualTo(1));

            record = webServiceClient.DatabaseRecords[0];
            Assert.That(record.RecordId, Is.EqualTo(unConfirmModel.Id));
            Assert.That(record.IsConfirmed(), Is.False);
        }

        [Test]
        public void UpdateModel()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);
            webServiceClient.GetViewFunc = ProductionViews.AreaValueModelView;
            Assert.That(webServiceClient.DatabaseRecords, Is.Empty);

            AmplaRepository<AreaValueModel> repository = new AmplaRepository<AreaValueModel>(webServiceClient, userName, password);
            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            repository.Add(model);
            Assert.That(webServiceClient.DatabaseRecords.Count, Is.EqualTo(1));

            InMemoryRecord record = webServiceClient.DatabaseRecords[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.GetFieldValue("Area", ""), Is.EqualTo("ROM"));
            Assert.That(record.GetFieldValue<double>("Value", 0), Is.EqualTo(100.0d));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.Not.EqualTo(DateTime.MinValue));

            Assert.That(model.Id, Is.EqualTo(record.RecordId));

            AreaValueModel updateModel = repository.FindById(model.Id);
            Assert.That(updateModel, Is.Not.Null);

            updateModel.Value = 200;
            repository.Update(updateModel);

            Assert.That(webServiceClient.DatabaseRecords.Count, Is.EqualTo(1));

            record = webServiceClient.DatabaseRecords[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.RecordId, Is.EqualTo(model.Id));
            Assert.That(record.GetFieldValue("Area", ""), Is.EqualTo("ROM"));
            Assert.That(record.GetFieldValue<double>("Value", 0), Is.EqualTo(200.0d));
        }

        [Test]
        public void UpdateModelWithNoChanges()
        {
            ListLogger listLogger = new ListLogger();
            
            SimpleDataWebServiceClient simpleWebServiceClient = new SimpleDataWebServiceClient(module, location);
            IDataWebServiceClient webServiceClient = new LoggingDataWebServiceClient(simpleWebServiceClient, listLogger);
            simpleWebServiceClient.GetViewFunc = ProductionViews.AreaValueModelView;
            Assert.That(simpleWebServiceClient.DatabaseRecords, Is.Empty);

            AmplaRepository<AreaValueModel> repository = new AmplaRepository<AreaValueModel>(webServiceClient, userName, password);
            AreaValueModel model = new AreaValueModel { Area = "ROM", Value = 100 };

            repository.Add(model);
            Assert.That(simpleWebServiceClient.DatabaseRecords.Count, Is.EqualTo(1));

            InMemoryRecord record = simpleWebServiceClient.DatabaseRecords[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.RecordId, Is.GreaterThan(0));
            Assert.That(record.GetFieldValue("Area", ""), Is.EqualTo("ROM"));
            Assert.That(record.GetFieldValue<double>("Value", 0), Is.EqualTo(100.0d));
            Assert.That(record.GetFieldValue("Sample Period", DateTime.MinValue), Is.Not.EqualTo(DateTime.MinValue));

            Assert.That(model.Id, Is.EqualTo(record.RecordId));

            AreaValueModel updateModel = repository.FindById(model.Id);
            Assert.That(updateModel, Is.Not.Null);

            int currentMessages = listLogger.Messages.Count;
            
            Assert.That(currentMessages, Is.GreaterThan(0));

            updateModel.Value = 100;
            repository.Update(updateModel);

            Assert.That(listLogger.Messages.Count, Is.EqualTo(currentMessages + 1));
            // record is not submitted
            Assert.That(simpleWebServiceClient.DatabaseRecords.Count, Is.EqualTo(1));

            record = simpleWebServiceClient.DatabaseRecords[0];
            Assert.That(record.Location, Is.EqualTo(location));
            Assert.That(record.RecordId, Is.EqualTo(model.Id));
            Assert.That(record.GetFieldValue("Area", ""), Is.EqualTo("ROM"));
            Assert.That(record.GetFieldValue<double>("Value", 0), Is.EqualTo(100.0d));
        }
    }
}