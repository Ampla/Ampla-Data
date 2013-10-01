using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Records;
using NUnit.Framework;

namespace AmplaWeb.Data.AmplaRepository
{
    [TestFixture]
    public class AmplaReadOnlyRepositoryUnitTests : TestFixture
    {
        [AmplaLocation(Location = "Plant.Area.Values")]
        [AmplaModule(Module = "Production")]
        public class AreaValueModel
        {
            public int Id { get; set; }
            public double Value { get; set; }
            public string Area { get; set; }
        }

        private ICredentialsProvider credentialsProvider = CredentialsProvider.ForUsernameAndPassword("User", "password");
        private const string module = "Production";
        private const string location = "Plant.Area.Values";

        [Test]
        public void GetAll()
        {
            IDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);
            AmplaReadOnlyRepository<AreaValueModel> repository = new AmplaReadOnlyRepository<AreaValueModel>(webServiceClient, credentialsProvider);
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
            
            AmplaReadOnlyRepository<AreaValueModel> repository = new AmplaReadOnlyRepository<AreaValueModel>(webServiceClient, credentialsProvider);
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

            AmplaReadOnlyRepository<AreaValueModel> repository = new AmplaReadOnlyRepository<AreaValueModel>(webServiceClient, credentialsProvider);
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

            AmplaReadOnlyRepository<AreaValueModel> repository = new AmplaReadOnlyRepository<AreaValueModel>(webServiceClient, credentialsProvider);
            AreaValueModel model = repository.FindById(recordId);

            Assert.That(model.Id, Is.EqualTo(recordId));
            Assert.That(model.Value, Is.EqualTo(0));
            Assert.That(model.Area, Is.EqualTo(null));

            Assert.That(repository.FindById(recordId + 1), Is.Null);
        }
    }
}