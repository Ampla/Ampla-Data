using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml;

using AmplaWeb.Data.Records;

namespace AmplaWeb.Data.AmplaData2008
{
    /// <summary>
    /// Simple implementation of the DataWebServiceClient
    /// </summary>
    public class SimpleDataWebServiceClient : IDataWebServiceClient
    {
        private readonly AmplaModules amplaModule;
        private readonly string module;
        private readonly Dictionary<int, InMemoryRecord> database = new Dictionary<int, InMemoryRecord>();
        private readonly string reportingPoint;

        private string userName = "User";
        private string password = "password";

        private int setId = 1000;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataWebServiceClient"/> class.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="location">The location.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">module</exception>
        public SimpleDataWebServiceClient(string module, string location)
        {
            if (!Enum.TryParse(module, out amplaModule))
            {
                throw new ArgumentOutOfRangeException("module");
            }
            this.module = Convert.ToString(amplaModule);
            reportingPoint = location;
        }


        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public GetDataResponse GetData(GetDataRequest request)
        {
            XmlDocument xmlDoc = new XmlDocument();
            List<InMemoryRecord> recordsToReturn = new List<InMemoryRecord>();
            string location = request.Filter.Location;

            CheckModule(request.View.Module);

            int? recordId = null;
            foreach (FilterEntry entry in request.Filter.Criteria)
            {
                if (entry.Name == "Id")
                {
                    recordId = int.Parse(entry.Value);
                }
            }

            foreach (InMemoryRecord amplaRecord in database.Values)
            {
                if (amplaRecord.Location == location)
                {
                    if ((recordId == null) || recordId.Value == amplaRecord.RecordId)
                    {
                        recordsToReturn.Add(amplaRecord);
                    }
                }
            }

            List<Row> rows = new List<Row>();
            foreach (InMemoryRecord record in recordsToReturn)
            {
                Row row = new Row {id = Convert.ToString(record.RecordId)};
                List<XmlElement> values = new List<XmlElement>();
                foreach (FieldValue value in record.Fields)
                {
                    string name = XmlConvert.EncodeName(value.Name);
                    XmlElement element = xmlDoc.CreateElement(name, "http://www.citect.com/Ampla/Data/2008/06");
                    element.InnerText = value.Value;
                    values.Add(element);
                }
                row.Any = values.ToArray();
                rows.Add(row);
            }

            GetDataResponse response = new GetDataResponse
                {
                    Context = new GetDataResponseContext
                        {
                            Context = request.View.Context,
                            Metadata = request.Metadata,
                            Mode = request.View.Mode,
                            Module = request.View.Module,
                            ResolveIdentifiers = request.OutputOptions.ResolveIdentifiers,
                            ViewName = request.View.Name,
                            Fields = request.View.Fields,
                            ModelFields = request.View.ModelFields,
                        },
                    RowSets = new[]
                        {
                            new RowSet
                                {
                                    Rows = rows.ToArray(),
                                    Columns = new FieldDefinition[0]
                                }
                        }
                };

            return response;
        }

        /// <summary>
        /// Gets the navigation hierarchy.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public GetNavigationHierarchyResponse GetNavigationHierarchy(GetNavigationHierarchyRequest request)
        {
            CheckModule(request.Module);

            Hierarchy hierarchy = new Hierarchy
            {
                module = amplaModule,
                context = NavigationContext.Plant,
                mode = NavigationMode.Location,
                ViewPoints = new[] { new ViewPoint() }
            };

            string[] parts = reportingPoint.Split('.');

            ViewPoint point = hierarchy.ViewPoints[0];

            for (int i = 0; i < parts.Length; i++)
            {
                string fullPath = string.Join(".", parts, 0, i + 1);
                if (i == parts.Length - 1)
                {
                    point.ViewPoints = new ViewPoint[0];
                    point.ReportingPoints = new[]
                                                {
                                                    new GetNavigationReportingPoint
                                                        {
                                                            id = fullPath,
                                                            DisplayName = parts[i],
                                                            LocalizedDisplayName = parts[i]
                                                        }
                                                };
                }
                else
                {
                    point.id = fullPath;
                    point.DisplayName = parts[i];
                    point.LocalizedDisplayName = parts[i];
                    point.ReportingPoints = new GetNavigationReportingPoint[0];
                    if (i < parts.Length - 2)
                    {
                        point.ViewPoints = new[] { new ViewPoint() };
                        point = point.ViewPoints[0];
                    }
                }
            }

            return new GetNavigationHierarchyResponse
            {
                Hierarchy = hierarchy,
                Context = new GetNavigationHierarchyResponseContext
                {
                    Module = (AmplaModules)Enum.Parse(typeof(AmplaModules), module),
                    Context = NavigationContext.Plant,
                    Mode = NavigationMode.Location,
                }
            };
        }

        /// <summary>
        /// Submits the data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SubmitDataResponse SubmitData(SubmitDataRequest request)
        {
            List<DataSubmissionResult> results = new List<DataSubmissionResult>();
            CheckCredentials(request.Credentials);
            foreach (SubmitDataRecord submitDataRecord in request.SubmitDataRecords)
            {
                CheckModule(submitDataRecord.Module);
                results.Add(submitDataRecord.MergeCriteria == null
                                ? InsertDataRecord(submitDataRecord)
                                : UpdateDataRecord(submitDataRecord));
            }
            return new SubmitDataResponse { DataSubmissionResults = results.ToArray() };
        }

        /// <summary>
        /// Deletes the records.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public DeleteRecordsResponse DeleteRecords(DeleteRecordsRequest request)
        {
            List<DeleteRecordsResult> results = new List<DeleteRecordsResult>();
            CheckCredentials(request.Credentials);
            foreach (DeleteRecord deleteRecord in request.DeleteRecords)
            {
                CheckModule(deleteRecord.Module);
                int recordId = (int)deleteRecord.MergeCriteria.SetId;
                InMemoryRecord record = FindRecord(deleteRecord.Location, deleteRecord.Module, recordId);
                FieldValue deleted = record.Find("Deleted");
                if (deleted != null)
                {
                    record.Fields.Remove(deleted);
                }
                record.Fields.Add(new FieldValue("Deleted", "True"));
                results.Add(new DeleteRecordsResult
                {
                    Location = deleteRecord.Location,
                    Module = deleteRecord.Module,
                    RecordAction = DeleteRecordsAction.Delete,
                    SetId = recordId
                });
            }
            return new DeleteRecordsResponse { DeleteRecordsResults = results.ToArray() };
        }

        /// <summary>
        /// Updates the record status.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public UpdateRecordStatusResponse UpdateRecordStatus(UpdateRecordStatusRequest request)
        {
            List<UpdateRecordStatusResult> results = new List<UpdateRecordStatusResult>();
            CheckCredentials(request.Credentials);
            foreach (UpdateRecordStatus recordStatus in request.UpdateRecords)
            {
                CheckModule(recordStatus.Module);
                int recordId = (int)recordStatus.MergeCriteria.SetId;
                InMemoryRecord record = FindRecord(recordStatus.Location, recordStatus.Module, recordId);
                FieldValue confirmed = record.Find("Confirmed");
                if (confirmed != null)
                {
                    record.Fields.Remove(confirmed);
                }
                string value = recordStatus.RecordAction == UpdateRecordStatusAction.Confirm ? "True" : "False";
                record.Fields.Add(new FieldValue("Confirmed", value));
                results.Add(new UpdateRecordStatusResult
                {
                    Location = recordStatus.Location,
                    Module = recordStatus.Module,
                    RecordAction = recordStatus.RecordAction,
                    SetId = recordId
                });
            }
            return new UpdateRecordStatusResponse { UpdateRecordStatusResults = results.ToArray() };
        }

        /// <summary>
        /// Gets the views.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public GetViewsResponse GetViews(GetViewsRequest request)
        {
            CheckModule(request.Module);
            GetViewsResponse response = new GetViewsResponse
            {
                Views = new[] { new GetView
                    {
                        Fields = new GetViewsField[0], 
                        AllowedOperations = new GetViewsAllowedOperation[0], 
                        Filters = new GetViewsFilter[0], 
                        Periods = new GetViewsPeriod[0]
                    } 
                }
            };
            return response;
        }

        /// <summary>
        /// Splits the records.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SplitRecordsResponse SplitRecords(SplitRecordsRequest request)
        {
            InMemoryRecord record = FindRecord(request.OriginalRecord.Location, request.OriginalRecord.Module,
                                            (int)request.OriginalRecord.SetId);
            InMemoryRecord[] splitRecords = record.SplitRecord(request.SplitRecords[0].SplitDateTimeUtc);

            splitRecords[1].RecordId = ++setId;

            database[splitRecords[0].RecordId] = splitRecords[0];
            database[splitRecords[1].RecordId] = splitRecords[1];

            return new SplitRecordsResponse
            {
                Context = new SplitRecordsResponseContext { OriginalRecord = request.OriginalRecord },
                SplitRecordResults =
                    new[]
                                   {
                                       new SplitRecordResult
                                           {
                                               StartDateTimeUtc =
                                                   splitRecords[0].GetFieldValue("Start Time", DateTime.MinValue),
                                               EndDateTimeUtc =
                                                   splitRecords[0].GetFieldValue("End Time", DateTime.MinValue),
                                               SetId = splitRecords[0].RecordId
                                           }
                                   }
            };
        }

        private DataSubmissionResult InsertDataRecord(SubmitDataRecord submitDataRecord)
        {
            setId++;

            InMemoryRecord amplaRecord = new InMemoryRecord { Location = submitDataRecord.Location, Module = module, RecordId = setId };

            foreach (Field field in submitDataRecord.Fields)
            {
                amplaRecord.Fields.Add(new FieldValue(field.Name, field.Value));
            }

            database[setId] = amplaRecord;
            return new DataSubmissionResult
            {
                RecordAction = RecordAction.Insert,
                SetId = setId
            };

        }

        private DataSubmissionResult UpdateDataRecord(SubmitDataRecord submitDataRecord)
        {
            int recordId = (int)submitDataRecord.MergeCriteria.SetId;

            InMemoryRecord record = FindRecord(submitDataRecord.Location, submitDataRecord.Module, recordId);

            foreach (Field field in submitDataRecord.Fields)
            {
                FieldValue fieldValue = record.Find(field.Name);
                if (fieldValue == null)
                {
                    record.Fields.Add(new FieldValue(field.Name, field.Value));
                }
                else
                {
                    fieldValue.Value = field.Value;
                }
            }

            return new DataSubmissionResult
            {
                RecordAction = RecordAction.Update,
                SetId = recordId
            };
        }

        private InMemoryRecord FindRecord(string searchLocation, AmplaModules searchModule, int searchRecordId)
        {
            InMemoryRecord record;
            if (!database.TryGetValue(searchRecordId, out record))
            {
                throw new FaultException("No record found with Id:" + searchRecordId);
            }

            if (record.Location != searchLocation || record.Module != searchModule.ToString())
            {
                string message = string.Format("No record found. (Module:'{0}', Location ='{1}')", searchModule,
                                               searchLocation);
                throw new FaultException(message);
            }
            return record;
        }


        private static void CheckCredentials(Credentials credentials)
        {
            if ((credentials.Username != "User") || (credentials.Password != "password"))
            {
                throw new ArgumentException("Invalid Credentials", "credentials");
            }
        }
        
        private void CheckModule(AmplaModules checkModule)
        {
            if (checkModule != amplaModule)
            {
                throw new ArgumentException("Invalid module: " + checkModule);
            }
        }

        public List<InMemoryRecord> DatabaseRecords
        {
            get
            {
                return new List<InMemoryRecord>(database.Values);
            }
        }

        public Credentials CreateCredentials()
        {
            return new Credentials { Username = userName, Password = password };
        }
    }
}