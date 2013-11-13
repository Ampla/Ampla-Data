
using System;
using System.ServiceModel;
using AmplaData.Data.Downtime;
using AmplaData.Data.Production;
using AmplaData.Data.Records;
using NUnit.Framework;

namespace AmplaData.Data.AmplaData2008
{
    public class SimpleDataWebServiceClientUnitTest : TestFixture
    {
        private static Credentials CreateCredentials()
        {
            return new Credentials { Username = "User", Password = "password" };
        }

        private const string location = "Enterprise.Site.Area.Production";
        private const string module = "Production";

        [Test]
        public void Insert()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);

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
        public void CreatedByDefault()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);

            InMemoryRecord record = ProductionRecords.NewRecord().MarkAsNew();

            SubmitDataRequest request = new SubmitDataRequest
            {
                Credentials = CreateCredentials(),
                SubmitDataRecords = new[]
                    {
                        record.ConvertToSubmitDataRecord()
                    }
            };

            DateTime before = DateTime.Now.AddSeconds(-10).ToUniversalTime();
            DateTime after = before.AddSeconds(+20);

            SubmitDataResponse response = webServiceClient.SubmitData(request);
            Assert.That(response, Is.Not.Null);

            Assert.That(webServiceClient.DatabaseRecords.Count, Is.EqualTo(1));
            InMemoryRecord inserted = webServiceClient.DatabaseRecords[0];

            Assert.That(inserted.GetFieldValue("CreatedBy", "null"), Is.EqualTo("User"));
            Assert.That(inserted.GetFieldValue("CreatedDateTime", DateTime.MinValue), Is.InRange(before, after));
        }

        [Test]
        public void CreatedBySetInRequest()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);

            InMemoryRecord record = ProductionRecords.NewRecord().MarkAsNew();
            record.SetFieldValue("CreatedBy", "UnitTests");
            record.SetFieldValue("CreatedDateTime", DateTime.Today);

            SubmitDataRequest request = new SubmitDataRequest
            {
                Credentials = CreateCredentials(),
                SubmitDataRecords = new[]
                    {
                        record.ConvertToSubmitDataRecord()
                    }
            };

            SubmitDataResponse response = webServiceClient.SubmitData(request);
            Assert.That(response, Is.Not.Null);

            Assert.That(webServiceClient.DatabaseRecords.Count, Is.EqualTo(1));
            InMemoryRecord inserted = webServiceClient.DatabaseRecords[0];

            Assert.That(inserted.GetFieldValue("CreatedBy", "null"), Is.EqualTo("UnitTests"));
            Assert.That(inserted.GetFieldValue("CreatedDateTime", DateTime.MinValue), Is.EqualTo(DateTime.Today.ToUniversalTime()));
        }

        [Test]
        public void InsertInvalidLocation()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);

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
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);

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
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);

            GetNavigationHierarchyResponse response = webServiceClient.GetNavigationHierarchy(new GetNavigationHierarchyRequest { Module = AmplaModules.Production });
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Hierarchy, Is.Not.Null);
            CheckViewPoints(response.Hierarchy.ViewPoints, "", "Enterprise");
            CheckViewPoints(response.Hierarchy.ViewPoints[0].ViewPoints, "Enterprise", "Site");
            CheckViewPoints(response.Hierarchy.ViewPoints[0].ViewPoints[0].ViewPoints, "Enterprise.Site", "Area");
            CheckViewPoints(response.Hierarchy.ViewPoints[0].ViewPoints[0].ViewPoints[0].ReportingPoints, "Enterprise.Site.Area", "Production");
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

            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(record.Module, record.Location);
                
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
        public void GetDataWithMetaDataReturnsColumns()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location)
                {
                    GetViewFunc = ProductionViews.StandardView
                };

            GetDataRequest request = new GetDataRequest
            {
                Credentials = CreateCredentials(),
                Filter = new DataFilter { Location = location, Criteria = new FilterEntry[0]},
                View = new GetDataView { Context = NavigationContext.Plant, Mode = NavigationMode.Location, Module = AmplaModules.Production },
                Metadata = true,
                OutputOptions = new GetDataOutputOptions { ResolveIdentifiers = false },
            };

          
            GetDataResponse response = webServiceClient.GetData(request);
            Assert.That(response.RowSets, Is.Not.Empty);
            Assert.That(response.RowSets[0].Columns, Is.Not.Empty);
        }

        [Test]
        public void GetDataWithNoMetaDataReturnsZeroColumns()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location)
                {
                    GetViewFunc = ProductionViews.StandardView
                };

            GetDataRequest request = new GetDataRequest
            {
                Credentials = CreateCredentials(),
                Filter = new DataFilter { Location = location, Criteria = new FilterEntry[0] },
                View = new GetDataView { Context = NavigationContext.Plant, Mode = NavigationMode.Location, Module = AmplaModules.Production },
                Metadata = false,
                OutputOptions = new GetDataOutputOptions { ResolveIdentifiers = false },
            };


            GetDataResponse response = webServiceClient.GetData(request);
            Assert.That(response.RowSets, Is.Not.Empty);
            Assert.That(response.RowSets[0].Columns, Is.Null);
        }

        [Test]
        public void GetDataReturnsLocation()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);

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

        [Test]
        public void GetAuditData()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);

            GetAuditDataRequest request = new GetAuditDataRequest
                {
                    Credentials = CreateCredentials(),
                    Filter = new GetAuditDataFilter {Location = location, Module = AmplaModules.Production}
                };

            GetAuditDataResponse response = webServiceClient.GetAuditData(request);
            Assert.That(response.RowSets, Is.Not.Empty);
            Assert.That(response.RowSets[0].Rows, Is.Empty);
        }

        [Test]
        public void GetAuditDataWithARecordWithNoChanges()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);

            InMemoryRecord record = ProductionRecords.NewRecord().MarkAsNew();

            SubmitDataRequest submitRequest = new SubmitDataRequest
            {
                Credentials = CreateCredentials(),
                SubmitDataRecords = new[]
                    {
                        record.ConvertToSubmitDataRecord()
                    }
            };

            webServiceClient.SubmitData(submitRequest);
            Assert.That(webServiceClient.DatabaseRecords, Is.Not.Empty);
            int recordId = webServiceClient.DatabaseRecords[0].RecordId;

            GetAuditDataRequest request = new GetAuditDataRequest
            {
                Credentials = CreateCredentials(),
                Filter = new GetAuditDataFilter { Location = location, Module = AmplaModules.Production, SetId = Convert.ToString(recordId)}
            };

            GetAuditDataResponse response = webServiceClient.GetAuditData(request);
            Assert.That(response.RowSets, Is.Not.Empty);
            Assert.That(response.RowSets[0].Rows, Is.Empty);
        }

        [Test]
        public void GetAuditDataWithARecordWithChanges()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(module, location);

            InMemoryRecord record = ProductionRecords.NewRecord().MarkAsNew();
            record.SetFieldValue("Field 1", 150);

            SubmitDataRequest submitRequest = new SubmitDataRequest
            {
                Credentials = CreateCredentials(),
                SubmitDataRecords = new[]
                    {
                        record.ConvertToSubmitDataRecord()
                    }
            };

            webServiceClient.SubmitData(submitRequest);

            Assert.That(webServiceClient.DatabaseRecords, Is.Not.Empty);
            int recordId = webServiceClient.DatabaseRecords[0].RecordId;

            InMemoryRecord updateRecord = new InMemoryRecord
                {
                    Location = record.Location,
                    Module = record.Module,
                    RecordId = recordId
                };
            updateRecord.SetFieldValue("Field 1", 200);

            SubmitDataRequest update = new SubmitDataRequest
                {
                    Credentials = CreateCredentials(),
                    SubmitDataRecords = new[]
                        {
                            updateRecord.ConvertToSubmitDataRecord()
                        }
                };

            webServiceClient.SubmitData(update);

            GetAuditDataRequest request = new GetAuditDataRequest
            {
                Credentials = CreateCredentials(),
                Filter = new GetAuditDataFilter { Location = location, Module = AmplaModules.Production, SetId = Convert.ToString(recordId) }
            };

            GetAuditDataResponse response = webServiceClient.GetAuditData(request);

            AssertAuditTableContains(response, location, recordId, "Field 1", "150", "200");
        }
        
        private static void AssertAuditTableContains(GetAuditDataResponse response, string objectId, int id,
                                              string field, string oldValue, string newValue)
        {
            Assert.That(response.RowSets, Is.Not.Empty);
            Assert.That(response.RowSets[0].Rows, Is.Not.Empty);

            int found = 0;
            string setId = Convert.ToString(id);
            foreach (GetAuditDataRow row in response.RowSets[0].Rows)
            {
                if (row.Field == field && row.Location == objectId && row.SetId == setId)
                {
                    Assert.That(row.EditedValue, Is.EqualTo(newValue));
                    Assert.That(row.OriginalValue, Is.EqualTo(oldValue));
                    found++;
                }
            }

            Assert.That(found, Is.EqualTo(1), "Field: {0} not found", field);
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