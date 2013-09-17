
using AmplaWeb.Data.Records;
using AmplaWeb.Data.Tests;
using NUnit.Framework;

namespace AmplaWeb.Data.AmplaData2008
{
    public class SimpleDataWebServiceClientUnitTest : TestFixture
    {
        private static Credentials CreateCredentials()
        {
            return new Credentials { Username = "User", Password = "password" };
        }

        [Test]
        public void Insert()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient("Production", "Plant.Area.Production");

            InMemoryRecord record = ProductionRecords.NewRecord().MarkAsNew();

            SubmitDataRequest request = new SubmitDataRequest
            {
                Credentials = CreateCredentials(),
                SubmitDataRecords = new[]
                    {
                        record.ConvertToSubmitDataRecord()
                    }
            };
            SubmitDataResponse response = webServiceClient.SubmitData(request);
            Assert.That(response.DataSubmissionResults, Is.Not.Null);
            Assert.That(response.DataSubmissionResults.Length, Is.EqualTo(1));
            Assert.That(response.DataSubmissionResults[0].RecordAction, Is.EqualTo(RecordAction.Insert));
            Assert.That(response.DataSubmissionResults[0].SetId, Is.GreaterThan(100));

            Assert.That(webServiceClient.DatabaseRecords.Count, Is.EqualTo(1));
        }

        [Test]
        public void Update()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient("Production",
                                                                                         "Plant.Area.Production");

            InMemoryRecord record = ProductionRecords.NewRecord().MarkAsNew();
            InMemoryRecord update = record.Clone();
            update.Fields.Clear();
            update.Fields.Add(new FieldValue("New Field", "100"));

            SubmitDataRequest request = new SubmitDataRequest
                {
                    Credentials = CreateCredentials(),
                    SubmitDataRecords = new[]
                        {
                            record.ConvertToSubmitDataRecord()
                        }
                };

            SubmitDataResponse response = webServiceClient.SubmitData(request);

            Assert.That(response.DataSubmissionResults, Is.Not.Null);
            Assert.That(response.DataSubmissionResults.Length, Is.EqualTo(1));
            Assert.That(response.DataSubmissionResults[0].RecordAction, Is.EqualTo(RecordAction.Insert));
            Assert.That(response.DataSubmissionResults[0].SetId, Is.GreaterThan(100));

            Assert.That(webServiceClient.DatabaseRecords.Count, Is.EqualTo(1));
            Assert.That(webServiceClient.DatabaseRecords[0].Find("New Field"), Is.Null);

            update.RecordId = (int) response.DataSubmissionResults[0].SetId;

            request = new SubmitDataRequest
                {
                    Credentials = CreateCredentials(),
                    SubmitDataRecords = new[]
                        {
                            update.ConvertToSubmitDataRecord()
                        }
                };

            response = webServiceClient.SubmitData(request);

            Assert.That(response.DataSubmissionResults, Is.Not.Null);
            Assert.That(response.DataSubmissionResults.Length, Is.EqualTo(1));
            Assert.That(response.DataSubmissionResults[0].RecordAction, Is.EqualTo(RecordAction.Update));
            Assert.That(response.DataSubmissionResults[0].SetId, Is.EqualTo(update.RecordId));

            Assert.That(webServiceClient.DatabaseRecords.Count, Is.EqualTo(1));
            Assert.That(webServiceClient.DatabaseRecords[0].Find("New Field"), Is.Not.Null);
            Assert.That(webServiceClient.DatabaseRecords[0].Find("New Field").Value, Is.EqualTo("100"));
        }

        [Test]
        public void GetNavigationHierarchy()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient("Production",
                                                                                         "Plant.Area.Production");

            GetNavigationHierarchyResponse response = webServiceClient.GetNavigationHierarchy(new GetNavigationHierarchyRequest { Module = AmplaModules.Production });
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Hierarchy, Is.Not.Null);
            CheckViewPoints(response.Hierarchy.ViewPoints, "", "Plant");
            CheckViewPoints(response.Hierarchy.ViewPoints[0].ViewPoints, "Plant", "Area");
            CheckViewPoints(response.Hierarchy.ViewPoints[0].ViewPoints[0].ReportingPoints, "Plant.Area", "Production");

        }

        private static void CheckViewPoints(ViewPoint[] points, string parent, string name)
        {
            Assert.That(points, Is.Not.Empty, parent);
            Assert.That(points.Length, Is.EqualTo(1), parent);
            Assert.That(points[0].DisplayName, Is.EqualTo(name), parent);
            Assert.That(points[0].LocalizedDisplayName, Is.EqualTo(name), parent);
            Assert.That(points[0].id, Is.EqualTo((parent.Length > 0 ? parent + "." : "") + name), parent);
            Assert.That(points[0].ViewPoints, Is.Not.Null, parent);
            Assert.That(points[0].ReportingPoints, Is.Not.Null, parent);
        }

        private static void CheckViewPoints(GetNavigationReportingPoint[] points, string parent, string name)
        {
            Assert.That(points, Is.Not.Empty, parent);
            Assert.That(points[0].DisplayName, Is.EqualTo(name), parent);
            Assert.That(points[0].LocalizedDisplayName, Is.EqualTo(name), parent);
            Assert.That(points[0].id, Is.EqualTo((parent.Length > 0 ? parent + "." : "") + name), parent);
        }

    }
}