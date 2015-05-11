
using System;
using System.Collections.Generic;
using System.ServiceModel;
using AmplaData.AmplaSecurity2007;
using AmplaData.Database;
using AmplaData.Modules.Downtime;
using AmplaData.Modules.Production;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.AmplaData2008
{
    public class SimpleDataWebServiceClientUnitTest : TestFixture
    {
        private static Credentials CreateCredentials()
        {
            return new Credentials { Username = "User", Password = "password" };
        }

        private const string location = "Enterprise.Site.Area.Production";
        private const string module = "Production";
        private SimpleAmplaDatabase database;
        private SimpleAmplaConfiguration configuration;

        private SimpleDataWebServiceClient Create()
        {
            return new SimpleDataWebServiceClient(database, configuration, new SimpleSecurityWebServiceClient("User"));
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            database = new SimpleAmplaDatabase();
            database.EnableModule(module);

            configuration = new SimpleAmplaConfiguration();
            configuration.EnableModule(module);
            configuration.AddLocation(module, location);
            configuration.SetDefaultView("Production", ProductionViews.StandardView());
        }

        protected List<InMemoryRecord> DatabaseRecords
        {
            get
            {
                return new List<InMemoryRecord>(database.GetModuleRecords(module).Values);
            }
        }
        
        [Test]
        public void Insert()
        {
            SimpleDataWebServiceClient webServiceClient = Create();

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

            Assert.That(DatabaseRecords.Count, Is.EqualTo(1));
        }

        [Test]
        public void CreatedByDefault()
        {
            SimpleDataWebServiceClient webServiceClient = Create();

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

            Assert.That(DatabaseRecords.Count, Is.EqualTo(1));
            InMemoryRecord inserted = DatabaseRecords[0];

            Assert.That(inserted.GetFieldValue("CreatedBy", "null"), Is.EqualTo("User"));
            Assert.That(inserted.GetFieldValue("CreatedDateTime", DateTime.MinValue), Is.InRange(before, after));
        }

        [Test]
        public void CreatedBySetInRequest()
        {
            SimpleDataWebServiceClient webServiceClient = Create();

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

            Assert.That(DatabaseRecords.Count, Is.EqualTo(1));
            InMemoryRecord inserted = DatabaseRecords[0];

            Assert.That(inserted.GetFieldValue("CreatedBy", "null"), Is.EqualTo("UnitTests"));
            Assert.That(inserted.GetFieldValue("CreatedDateTime", DateTime.MinValue), Is.EqualTo(DateTime.Today.ToUniversalTime()));
        }

        [Test]
        public void GetDataInvalidUser()
        {
            SimpleDataWebServiceClient webServiceClient = Create();

            GetDataRequest invalidRequest = new GetDataRequest
            {
                Credentials = new Credentials {Username = "Invalid.User", Password = "password"},
                Filter = new DataFilter {Location = location},
                View = new GetDataView { Module = AmplaModules.Production, Context = NavigationContext.Plant}
            };

            Assert.Throws<FaultException>(() => webServiceClient.GetData(invalidRequest));
        }

        [Test]
        public void GetDataWithNoData()
        {
            SimpleDataWebServiceClient webServiceClient = Create();

            GetDataRequest request = new GetDataRequest
            {
                Credentials = CreateCredentials(),
                Filter = new DataFilter { Location = location },
                Metadata = true,
               // OutputOptions = new GetDataOutputOptions(),
                View = new GetDataView { Module = AmplaModules.Production, Context = NavigationContext.Plant }
            };

            var response = webServiceClient.GetData(request);
            Assert.That(response.RowSets, Is.Not.Empty);
        }


        [Test]
        public void InsertInvalidLocation()
        {
            SimpleDataWebServiceClient webServiceClient = Create();
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
            SimpleDataWebServiceClient webServiceClient = Create();

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

            Assert.That(DatabaseRecords.Count, Is.EqualTo(1));
            Assert.That(DatabaseRecords[0].Find("New Field"), Is.Null);

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

            Assert.That(DatabaseRecords.Count, Is.EqualTo(1));
            Assert.That(DatabaseRecords[0].Find("New Field"), Is.Not.Null);
            Assert.That(DatabaseRecords[0].Find("New Field").Value, Is.EqualTo("100"));
        }

        [Test]
        public void GetNavigationHierarchy()
        {
            SimpleDataWebServiceClient webServiceClient = Create();

            GetNavigationHierarchyResponse response = webServiceClient.GetNavigationHierarchy(
                new GetNavigationHierarchyRequest
                    {
                        Module = AmplaModules.Production,
                        Credentials = CreateCredentials()
                    });
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
            configuration = new SimpleAmplaConfiguration();
            configuration.EnableModule(module);
            configuration.AddLocation(module, "Plant.Area.Production");
            configuration.AddLocation(module, "Plant.Area.Equipment.Production");

            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(
                database, 
                configuration, 
                new SimpleSecurityWebServiceClient("User"));

            GetNavigationHierarchyResponse response = webServiceClient.GetNavigationHierarchy(
                new GetNavigationHierarchyRequest
                    {
                       Module = AmplaModules.Production,
                       Credentials = CreateCredentials()
                    });
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

            database.EnableModule("Downtime");

            configuration.EnableModule("Downtime");
            configuration.AddLocation("Downtime", "Enterprise.Site.Area.Downtime");
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(database, configuration, new SimpleSecurityWebServiceClient("User"));

            record.SaveTo(webServiceClient);

            List<InMemoryRecord> records = new List<InMemoryRecord>(database.GetModuleRecords("Downtime").Values);
            Assert.That(records, Is.Not.Empty);

            string recordId = Convert.ToString(records[0].RecordId);

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

            database.EnableModule("Downtime");
            configuration.EnableModule("Downtime");
            configuration.AddLocation("Downtime", "Enterprise.Site.Area.Downtime");
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(database, configuration, new SimpleSecurityWebServiceClient("User"));
   
            record.SaveTo(webServiceClient);

            List<InMemoryRecord> records = new List<InMemoryRecord>(database.GetModuleRecords("Downtime").Values);
            Assert.That(records, Is.Not.Empty);
            
            string recordId = Convert.ToString(records[0].RecordId);

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
            SimpleDataWebServiceClient webServiceClient = Create();
        
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
        public void UseSessionIdInCredentials()
        {
            SimpleSecurityWebServiceClient securityWebService = new SimpleSecurityWebServiceClient("User");

            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(database, configuration, securityWebService );
            Assert.Throws<FaultException>(() =>
                                          webServiceClient.GetNavigationHierarchy(new GetNavigationHierarchyRequest()
                                              {
                                                  Module = AmplaModules.Production,
                                                  Credentials = new Credentials {Session = Guid.NewGuid().ToString()}
                                              }));

            securityWebService.AddExistingSession("User");

            Assert.That(securityWebService.Sessions, Is.Not.Empty);
            SimpleSession session = securityWebService.Sessions[0];

            webServiceClient.GetNavigationHierarchy(new GetNavigationHierarchyRequest()
                {
                    Module = AmplaModules.Production,
                    Credentials = new Credentials {Session = session.SessionId}
                });
        }

        [Test]
        public void GetDataWithNoMetaDataReturnsZeroColumns()
        {
            SimpleDataWebServiceClient webServiceClient = Create();

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
        public void GetDataWithNullOutputOptions()
        {
            SimpleDataWebServiceClient webServiceClient = Create();

            GetDataRequest request = new GetDataRequest
            {
                Credentials = CreateCredentials(),
                Filter = new DataFilter { Location = location, Criteria = new FilterEntry[0] },
                View = new GetDataView { Context = NavigationContext.Plant, Mode = NavigationMode.Location, Module = AmplaModules.Production },
                Metadata = false,
                OutputOptions = null
            };

            GetDataResponse response = webServiceClient.GetData(request);
            Assert.That(response.RowSets, Is.Not.Empty);
            Assert.That(response.RowSets[0].Columns, Is.Null);
            Assert.That(response.Context.ResolveIdentifiers, Is.False);
        }
        
        [Test]
        public void GetDataReturnsLocation()
        {
            SimpleDataWebServiceClient webServiceClient = new SimpleDataWebServiceClient(
                database, configuration, new SimpleSecurityWebServiceClient("User"));

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
            Assert.That(DatabaseRecords.Count, Is.EqualTo(1));

            Assert.That(DatabaseRecords[0].Location, Is.EqualTo(location));

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
            SimpleDataWebServiceClient webServiceClient = Create();

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
            SimpleDataWebServiceClient webServiceClient = Create();

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
            Assert.That(DatabaseRecords, Is.Not.Empty);
            int recordId = DatabaseRecords[0].RecordId;

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
            SimpleDataWebServiceClient webServiceClient = Create();

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

            Assert.That(DatabaseRecords, Is.Not.Empty);
            int recordId = DatabaseRecords[0].RecordId;

            InMemoryRecord updateRecord = new InMemoryRecord(ProductionViews.StandardView())
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