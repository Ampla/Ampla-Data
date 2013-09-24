using System;
using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Logging;
using AmplaWeb.Data.Records;
using AmplaWeb.Data.Tests;
using NUnit.Framework;

namespace AmplaWeb.Data.AmplaRepository
{
    [TestFixture]
    public abstract class AmplaRepositoryTestFixture<TModel> : TestFixture where TModel : class, new()
    {
        private readonly string module;
        private readonly string location;
        private readonly Func<GetView> getViewFunc;
        private const string userName = "User";
        private const string password = "password";

        private AmplaRepository<TModel> repository;
        private SimpleDataWebServiceClient webServiceClient;
        private ListLogger listLogger;

        protected AmplaRepositoryTestFixture(string module, string location, Func<GetView> getViewFunc)
        {
            this.module = module;
            this.location = location;
            this.getViewFunc = getViewFunc;
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            webServiceClient = new SimpleDataWebServiceClient(module, location);
            webServiceClient.GetViewFunc = getViewFunc;
            listLogger = new ListLogger();
            repository = new AmplaRepository<TModel>(new LoggingDataWebServiceClient(webServiceClient, listLogger), userName, password);
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
            get { return webServiceClient.DatabaseRecords; }
        }

        protected int SaveRecord(InMemoryRecord record)
        {
            return record.SaveTo(webServiceClient);
        }

        protected IList<string> Messages
        {
            get { return listLogger.Messages; }
        }
    }
}