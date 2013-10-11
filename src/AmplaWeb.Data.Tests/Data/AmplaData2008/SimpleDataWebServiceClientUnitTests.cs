
using System;
using System.ServiceModel;
using AmplaWeb.Data.Downtime;
using AmplaWeb.Data.Production;
using AmplaWeb.Data.Records;
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
        public void InsertInvalidLocation()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient("Production", "Plant.Area.Production");

            InMemoryRecord record = ProductionRecords.NewRecord().MarkAsNew();
            record.Location = record.Location + ".Invalid";

            SubmitDataRequest request = new SubmitDataRequest
            {
                Credentials = CreateCredentials(),
                SubmitDataRecords = new[]
                    {
                        record.ConvertToSubmitDataRecord()
                    }
            };

            Assert.Throws<FaultException>(() => webServiceClient.SubmitData(request));
        }

        [Test]
        public void Update()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient("Production",
                                                                                         "Plant.Area.Production");

            InMemoryRecord record = ProductionRecords.NewRecord().MarkAsNew();
            InMemoryRecord update = record.Clone();
            update.Fields.Clear();
            update.Location = record.Location;
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

        [Test]
        public void GetNavigationHierarchyTwoLocations()
        {
            string[] locations = new [] { "Plant.Area.Production", "Plant.Area.Equipment.Production"};
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient("Production",
                                                                                         locations);

            GetNavigationHierarchyResponse response = webServiceClient.GetNavigationHierarchy(new GetNavigationHierarchyRequest { Module = AmplaModules.Production });
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Hierarchy, Is.Not.Null);
            CheckViewPoints(response.Hierarchy.ViewPoints, "", "Plant");
            CheckViewPoints(response.Hierarchy.ViewPoints[0].ViewPoints, "Plant", "Area");
            CheckViewPoints(response.Hierarchy.ViewPoints[0].ViewPoints[0].ViewPoints, "Plant.Area", "Equipment");

            CheckViewPoints(response.Hierarchy.ViewPoints[0].ViewPoints[0].ReportingPoints, "Plant.Area", "Production");
            CheckViewPoints(response.Hierarchy.ViewPoints[0].ViewPoints[0].ViewPoints[0].ReportingPoints, "Plant.Area.Equipment", "Production");
        }

        [Test]
        public void GetData_ResolveIdentifiers_True()
        {
            InMemoryRecord record = DowntimeRecords.NewRecord().MarkAsNew();
            record.SetFieldIdValue("Cause", "Shutdown", 100);
            record.SetFieldIdValue("Classification", "Unplanned Process", 200);

            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(record.Module,
                                                                                         record.Location);
            record.SaveTo(webServiceClient);

            Assert.That(webServiceClient.DatabaseRecords, Is.Not.Empty);

            string recordId = Convert.ToString(webServiceClient.DatabaseRecords[0].RecordId);

            GetDataRequest request = new GetDataRequest
                {
                    Credentials = CreateCredentials(),
                    Filter = new DataFilter {Location = record.Location, Criteria = new [] { new FilterEntry {Name = "Id", Value = recordId}}},
                    View = new GetDataView { Context = NavigationContext.Plant, Mode = NavigationMode.Location, Module = AmplaModules.Downtime},
                    Metadata = true,
                    OutputOptions = new GetDataOutputOptions {ResolveIdentifiers = true}
                };
            GetDataResponse response = webServiceClient.GetData(request);

            AssertResponseContainsValue(response, "Cause", "Shutdown");
            AssertResponseContainsValue(response, "Classification", "Unplanned Process");
        }

        [Test]
        public void GetData_ResolveIdentifiers_False()
        {
            InMemoryRecord record = DowntimeRecords.NewRecord().MarkAsNew();
            record.SetFieldIdValue("Cause", "Shutdown", 100);
            record.SetFieldIdValue("Classification", "Unplanned Process", 200);

            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(record.Module,
                                                                                         record.Location);
            record.SaveTo(webServiceClient);

            Assert.That(webServiceClient.DatabaseRecords, Is.Not.Empty);

          
            string recordId = Convert.ToString(webServiceClient.DatabaseRecords[0].RecordId);

            GetDataRequest request = new GetDataRequest
            {
                Credentials = CreateCredentials(),
                Filter = new DataFilter { Location = record.Location, Criteria = new[] { new FilterEntry { Name = "Id", Value = recordId } } },
                View = new GetDataView { Context = NavigationContext.Plant, Mode = NavigationMode.Location, Module = AmplaModules.Downtime },
                Metadata = true,
                OutputOptions = new GetDataOutputOptions { ResolveIdentifiers = false },
            };
            GetDataResponse response = webServiceClient.GetData(request);
            AssertResponseContainsValue(response, "Cause", "100");
            AssertResponseContainsValue(response, "Classification", "200");
        }

        [Test]
        public void GetDataReturnsLocation()
        {
            const string location = "Plant.Area.Production";

            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient("Production", location);

            InMemoryRecord record = ProductionRecords.NewRecord().MarkAsNew();

            SubmitDataRequest submitRequest = new SubmitDataRequest
            {
                Credentials = CreateCredentials(),
                SubmitDataRecords = new[]
                    {
                        record.ConvertToSubmitDataRecord()
                    }
            };
            SubmitDataResponse submitResponse = webServiceClient.SubmitData(submitRequest);
            Assert.That(submitResponse.DataSubmissionResults, Is.Not.Null);
            Assert.That(submitResponse.DataSubmissionResults.Length, Is.EqualTo(1));
            Assert.That(submitResponse.DataSubmissionResults[0].RecordAction, Is.EqualTo(RecordAction.Insert));
            Assert.That(submitResponse.DataSubmissionResults[0].SetId, Is.GreaterThan(100));

            string recordId = Convert.ToString(submitResponse.DataSubmissionResults[0].SetId);
            Assert.That(webServiceClient.DatabaseRecords.Count, Is.EqualTo(1));

            Assert.That(webServiceClient.DatabaseRecords[0].Location, Is.EqualTo(location));

            GetDataRequest request = new GetDataRequest
            {
                Credentials = CreateCredentials(),
                Filter = new DataFilter { Location = record.Location, Criteria = new[] { new FilterEntry { Name = "Id", Value = recordId } } },
                View = new GetDataView { Context = NavigationContext.Plant, Mode = NavigationMode.Location, Module = AmplaModules.Production },
                Metadata = true,
                OutputOptions = new GetDataOutputOptions { ResolveIdentifiers = false },
            };
            GetDataResponse response = webServiceClient.GetData(request);

            AssertResponseContainsValue(response, "Duration", "90");
            AssertResponseContainsValue(response, "Location", location);
        }

        private void AssertResponseContainsValue(GetDataResponse response, string field, string value)
        {
            Assert.That(response.RowSets, Is.Not.Empty);
            Assert.That(response.RowSets[0].Rows, Is.Not.Empty);
            Assert.That(response.RowSets[0].Rows[0].Any, Is.Not.Empty);
            int found = 0; 
            foreach (var rowValue in response.RowSets[0].Rows[0].Any)
            {
                if (rowValue.Name == field)
                {
                    Assert.That(rowValue.InnerText, Is.EqualTo(value), "Field {0} value is not set", field);
                    found++;
                }
            }
            Assert.That(found, Is.EqualTo(1), "Field: {0} not found", field);
        }


        private static void CheckViewPoints(ViewPoint[] points, string parent, params string[] names)
        {
            Assert.That(points, Is.Not.Empty, parent);
            Assert.That(points.Length, Is.EqualTo(names.Length), parent);
            for (int i = 0; i < names.Length; i++)
            {
                string name = names[0];

                Assert.That(points[i].DisplayName, Is.EqualTo(name), "[{0}] {1}", i, parent);
                Assert.That(points[i].LocalizedDisplayName, Is.EqualTo(name), "[{0}] {1}", i, parent);
                Assert.That(points[i].id, Is.EqualTo((parent.Length > 0 ? parent + "." : "") + name), "[{0}] {1}", i, parent);
                Assert.That(points[i].ViewPoints, Is.Not.Null, "[{0}] {1}", i, parent);
                Assert.That(points[i].ReportingPoints, Is.Not.Null, "[{0}] {1}", i, parent);
            }
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