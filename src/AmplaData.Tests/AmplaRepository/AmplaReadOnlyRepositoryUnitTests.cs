using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.AmplaSecurity2007;
using AmplaData.Attributes;
using AmplaData.Database;
using AmplaData.Modules.Production;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.AmplaRepository
{
    [TestFixture]
    public class AmplaReadOnlyRepositoryUnitTests : TestFixture
    {
        [AmplaLocation(Location = "Plant.Area.Values")]
        [AmplaModule(Module = "Production")]
        private class AreaValueModel
        {
            public int Id { get; set; }
            public double Value { get; set; }
            public string Area { get; set; }
        }

        private readonly ICredentialsProvider credentialsProvider = CredentialsProvider.ForUsernameAndPassword("User", "password");
        private AmplaReadOnlyRepository<AreaValueModel> repository;
        private SimpleDataWebServiceClient webServiceClient;
        private const string module = "Production";
        private const string location = "Plant.Area.Values";
        private SimpleAmplaDatabase database;
        private SimpleAmplaConfiguration configuration;

        protected override void OnSetUp()
        {
            base.OnSetUp();
            database = new SimpleAmplaDatabase();
            database.EnableModule(module);

            configuration = new SimpleAmplaConfiguration();
            configuration.EnableModule(module);
            configuration.AddLocation(module, location);

            webServiceClient = new SimpleDataWebServiceClient(database, configuration, new SimpleSecurityWebServiceClient("User")) {GetViewFunc = ProductionViews.AreaValueModelView};
            repository = new AmplaReadOnlyRepository<AreaValueModel>(new AmplaRepository<AreaValueModel>(webServiceClient, credentialsProvider));
        }

        protected override void OnTearDown()
        {
            repository = null;
            webServiceClient = null;
            database = null;
            base.OnTearDown();
        }

        protected List<InMemoryRecord> DatabaseRecords
        {
            get
            {
                return new List<InMemoryRecord>(database.GetModuleRecords(module).Values);
            }
        }

        [Test]
        public void GetAll()
        {
            IList<AreaValueModel> models = repository.GetAll();

            Assert.That(models, Is.Empty);
        }

        [Test]
        public void GetAllWithExistingRecord()
        {
            InMemoryRecord record = ProductionRecords.NewRecord();
            record.Location = location;
            record.MarkAsNew();

            int recordId = record.SaveTo(webServiceClient);
            Assert.That(recordId, Is.GreaterThan(0));

            Assert.That(DatabaseRecords, Is.Not.Empty);
            
            IList<AreaValueModel> models = repository.GetAll();

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

            int recordId = record.SaveTo(webServiceClient);
            Assert.That(recordId, Is.GreaterThan(0));

            Assert.That(DatabaseRecords, Is.Not.Empty);

            AreaValueModel model = repository.FindById(recordId);

            Assert.That(model.Id, Is.EqualTo(recordId));
            Assert.That(model.Value, Is.EqualTo(100));
            Assert.That(model.Area, Is.EqualTo("ROM"));

            Assert.That(repository.FindById(recordId + 1), Is.Null);
        }


        [Test]
        public void FindByIdForDefaultModel()
        {
            InMemoryRecord record = ProductionRecords.NewRecord();
            //record.SetFieldValue("Value", 100);
            //record.SetFieldValue("Area", "ROM");
            record.Location = location;
            record.MarkAsNew();

            int recordId = record.SaveTo(webServiceClient);
            Assert.That(recordId, Is.GreaterThan(0));

            Assert.That(DatabaseRecords, Is.Not.Empty);
            AreaValueModel model = repository.FindById(recordId);

            Assert.That(model.Id, Is.EqualTo(recordId));
            Assert.That(model.Value, Is.EqualTo(0));
            Assert.That(model.Area, Is.EqualTo(null));

            Assert.That(repository.FindById(recordId + 1), Is.Null);
        }
    }
}