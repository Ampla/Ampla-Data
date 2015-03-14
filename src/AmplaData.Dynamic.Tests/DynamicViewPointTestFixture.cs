using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.AmplaSecurity2007;
using AmplaData.Modules.Production;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.Dynamic
{
    [TestFixture]
    public abstract class DynamicViewPointTestFixture : TestFixture
    {
        private readonly string location;
        private readonly string module;
        private SimpleDataWebServiceClient webServiceClient;
        private DynamicViewPoint dynamicViewPoint;

        protected DynamicViewPointTestFixture(string location, string module)
        {
            this.location = location;
            this.module = module;
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            SimpleSecurityWebServiceClient securityWebService = new SimpleSecurityWebServiceClient("User");
            webServiceClient = new SimpleDataWebServiceClient(module, location, securityWebService)
            {
                GetViewFunc = ProductionViews.StandardView
            };

            DataWebServiceFactory.Factory = () => webServiceClient;

            dynamicViewPoint = new DynamicViewPoint(location, module);
        }

        protected override void OnTearDown()
        {
            base.OnTearDown();
            DataWebServiceFactory.Factory = null;
        }

        protected int AddExisingRecord(InMemoryRecord record)
        {
            return webServiceClient.AddExistingRecord(record);
        }

        protected dynamic DynamicViewPoint
        {
            get { return dynamicViewPoint; }
        }

        protected DynamicViewPoint ViewPoint
        {
            get { return dynamicViewPoint; }
        }

        protected List<InMemoryRecord> Records
        {
            get { return webServiceClient.DatabaseRecords; }
        }
        
    }
}