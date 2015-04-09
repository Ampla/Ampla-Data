using System;
using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.AmplaSecurity2007;
using AmplaData.Logging;
using AmplaData.Records;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace AmplaData.AmplaRepository
{
    [TestFixture]
    public abstract class AmplaRepositoryTestFixture<TModel> : TestFixture where TModel : class, new()
    {
        private readonly string module;
        private readonly string[] locations;
        private readonly Func<GetView> getViewFunc;
        private const string userName = "User";
        private const string password = "password";

        private AmplaRepository<TModel> repository;
        private SimpleDataWebServiceClient webServiceClient;
        private ListLogger listLogger;
        private SimpleAmplaDatabase database;

        protected AmplaRepositoryTestFixture(string module, string[] locations, Func<GetView> getViewFunc)
        {
            this.module = module;
            this.locations = locations;
            this.getViewFunc = getViewFunc;
        }

        protected AmplaRepositoryTestFixture(string module, string location, Func<GetView> getViewFunc) 
            : this(module, new [] {location}, getViewFunc)
        {
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            database = new SimpleAmplaDatabase();
            database.EnableModule(module);
            webServiceClient = new SimpleDataWebServiceClient(database, module, locations, new SimpleSecurityWebServiceClient("User")) {GetViewFunc = getViewFunc};
            listLogger = new ListLogger();
            repository = new AmplaRepository<TModel>(new LoggingDataWebServiceClient(webServiceClient, listLogger), 
                CredentialsProvider.ForUsernameAndPassword(userName, password));
        }

        protected override void OnTearDown()
        {
            base.OnTearDown();
            webServiceClient = null;
            listLogger = null;
            if (repository != null)
            {
                repository.Dispose();
                repository = null;
            }
        }

        protected AmplaRepository<TModel> Repository
        {
            get { return repository; }
        }

        protected List<InMemoryRecord> Records
        {
            get { return new List<InMemoryRecord>(database.GetModuleRecords(module).Values); }
        }

        protected int SaveRecord(InMemoryRecord record)
        {
            return record.SaveTo(webServiceClient);
        }

        protected int UpdateRecord(InMemoryRecord record)
        {
            SubmitDataRequest request = new SubmitDataRequest
                {
                    Credentials = new Credentials { Username = "User", Password = "password"},
                    SubmitDataRecords = new[] {record.ConvertToSubmitDataRecord()}
                };

            SubmitDataResponse response = webServiceClient.SubmitData(request);
            Assert.That(response.DataSubmissionResults, Is.Not.Empty);
            Assert.That(response.DataSubmissionResults[0].RecordAction, Is.EqualTo(RecordAction.Update), "Expected an Update to occur");
            return (int) response.DataSubmissionResults[0].SetId;
        }

        protected IList<string> Messages
        {
            get { return listLogger.Messages; }
        }


        protected void AssertModelVersionProperty(ModelVersions modelVersions,
                                                               int index,
                                                               Func<TModel, object> modelFunc,
                                                               IResolveConstraint expectedConstraint)
        {
            Assert.That(modelVersions.Versions.Count, Is.GreaterThan(index), "Unable to find Version {0}", index);
            ModelVersion modelVersion = modelVersions.Versions[index];
            ModelVersion<TModel> typedModel = (ModelVersion<TModel>)modelVersion;
            Assert.That(modelFunc(typedModel.Model), expectedConstraint);
        }
    }
}