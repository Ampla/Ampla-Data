using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Xml;
using AmplaData.Records;
using AmplaData.Records.Filters;
using AmplaData.Views;

namespace AmplaData.AmplaData2008
{
    /// <summary>
    /// Simple implementation of the DataWebServiceClient
    /// </summary>
    public class SimpleDataWebServiceClient : IDataWebServiceClient
    {
        private readonly AmplaModules amplaModule;
        private readonly string module;
        private readonly Dictionary<int, InMemoryRecord> database = new Dictionary<int, InMemoryRecord>();
        private readonly List<InMemoryAuditRecord> auditDatabase = new List<InMemoryAuditRecord>();
        private readonly string[] reportingPoints;

        private const string userName = "User";
        private const string password = "password";

        private int setId = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataWebServiceClient"/> class.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="location">The location.</param>
        public SimpleDataWebServiceClient(string module, string location) : this(module, new [] {location})
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataWebServiceClient"/> class.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="locations">The valid locations.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">module</exception>
        public SimpleDataWebServiceClient(string module, string[] locations)
        {
            if (!Enum.TryParse(module, out amplaModule))
            {
                throw new ArgumentOutOfRangeException("module");
            }
            this.module = Convert.ToString(amplaModule);
            reportingPoints = locations;

            GetViewFunc = StandardViews.EmptyView;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public GetDataResponse GetData(GetDataRequest request)
        {
            return TryCatchThrowFault(() =>
                {
                    XmlDocument xmlDoc = new XmlDocument();

                    CheckModule(request.View.Module);

                    InMemoryFilterMatcher filterMatcher = new InMemoryFilterMatcher(request.Filter);

                    List<InMemoryRecord> recordsToReturn = database.Values.Where(filterMatcher.Matches).ToList();
                    bool resolveIdentifiers = request.OutputOptions.ResolveIdentifiers;

                    List<Row> rows = new List<Row>();
                    foreach (InMemoryRecord record in recordsToReturn)
                    {
                        Row row = new Row {id = Convert.ToString(record.RecordId)};
                        List<XmlElement> values = new List<XmlElement>();
                        foreach (FieldValue value in record.Fields)
                        {
                            string name = XmlConvert.EncodeName(value.Name);
                            XmlElement element = xmlDoc.CreateElement(name, "http://www.citect.com/Ampla/Data/2008/06");
                            element.InnerText = value.ResolveValue(resolveIdentifiers);
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
                                            Columns = request.Metadata ? GetColumns() : null
                                        }
                                }
                        };

                    return response;
                });
        }

        public GetAuditDataResponse GetAuditData(GetAuditDataRequest request)
        {
            CheckCredentials(request.Credentials);
            CheckModule(request.Filter.Module);
            
            InMemoryFilterMatcher filterMatcher = new InMemoryFilterMatcher(request.Filter);

            List<InMemoryAuditRecord> auditRecords = auditDatabase.Where(filterMatcher.Matches).ToList();

            List<GetAuditDataRow> rows = new List<GetAuditDataRow>();
            
            foreach (InMemoryAuditRecord record in auditRecords)
            {
                GetAuditDataRow row = new GetAuditDataRow
                    {
                        EditedBy = record.EditedBy,
                        EditedDateTime = record.EditedDateTime,
                        EditedValue = record.EditedValue,
                        Field = record.Field,
                        Location = record.Location,
                        OriginalValue = record.OriginalValue,
                        RecordType = record.RecordType,
                        SetId = record.SetId
                    };

                rows.Add(row);
            }

            GetAuditDataResponse response = new GetAuditDataResponse
                {
                    Context = new GetAuditDataResponseContext
                        {
                            Filter = new GetAuditDataResponseFilter
                                {
                                    Module = request.Filter.Module,
                                    Location = request.Filter.Location,
                                    RecordType = request.Filter.RecordType,
                                    SetId = request.Filter.SetId,
                                    EditedBy = request.Filter.EditedBy,
                                    EditedSamplePeriod = null
                                }
                        },
                    RowSets = new[]
                        {
                            new GetAuditDataRowSet
                                {
                                    Rows = rows.ToArray(),
                                }
                        }
                };
            return response;
        }

        private FieldDefinition[] GetColumns()
        {
            List<FieldDefinition> columns = new List<FieldDefinition>();
            GetView view = GetViewFunc();
            foreach (GetViewsField field in view.Fields)
            {
                columns.Add(new FieldDefinition
                    {
                        name = field.name,
                        displayName = field.displayName,
                        type = field.type
                    });
            }
            return columns.ToArray();
        }

        /// <summary>
        /// Gets the navigation hierarchy.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public GetNavigationHierarchyResponse GetNavigationHierarchy(GetNavigationHierarchyRequest request)
        {
            return TryCatchThrowFault(() =>
                {
                    CheckModule(request.Module);

                    SimpleNavigationHierarchy navigationHierarchy = new SimpleNavigationHierarchy(amplaModule, reportingPoints);

                    return new GetNavigationHierarchyResponse
                        {
                            Hierarchy = navigationHierarchy.GetHierarchy(),
                            Context = new GetNavigationHierarchyResponseContext
                                {
                                    Module = (AmplaModules) Enum.Parse(typeof (AmplaModules), module),
                                    Context = NavigationContext.Plant,
                                    Mode = NavigationMode.Location,
                                }
                        };
                });
        }

        /// <summary>
        /// Submits the data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SubmitDataResponse SubmitData(SubmitDataRequest request)
        {
            return TryCatchThrowFault(() =>
                {
                    List<DataSubmissionResult> results = new List<DataSubmissionResult>();
                    CheckCredentials(request.Credentials);
                    foreach (SubmitDataRecord submitDataRecord in request.SubmitDataRecords)
                    {
                        CheckModule(submitDataRecord.Module);
                        CheckReportingPoint(submitDataRecord.Location);
                        results.Add(submitDataRecord.MergeCriteria == null
                                        ? InsertDataRecord(submitDataRecord)
                                        : UpdateDataRecord(submitDataRecord));
                    }

                    return new SubmitDataResponse {DataSubmissionResults = results.ToArray()};
                });
        }

        /// <summary>
        /// Deletes the records.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public DeleteRecordsResponse DeleteRecords(DeleteRecordsRequest request)
        {
            return TryCatchThrowFault(() =>
                {
                    List<DeleteRecordsResult> results = new List<DeleteRecordsResult>();
                    CheckCredentials(request.Credentials);
                    foreach (DeleteRecord deleteRecord in request.DeleteRecords)
                    {
                        CheckModule(deleteRecord.Module);
                        CheckReportingPoint(deleteRecord.Location);
                        int recordId = (int) deleteRecord.MergeCriteria.SetId;
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
                    return new DeleteRecordsResponse {DeleteRecordsResults = results.ToArray()};
                });
        }

        /// <summary>
        /// Updates the record status.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public UpdateRecordStatusResponse UpdateRecordStatus(UpdateRecordStatusRequest request)
        {
            return TryCatchThrowFault(() =>
                {
                    List<UpdateRecordStatusResult> results = new List<UpdateRecordStatusResult>();
                    CheckCredentials(request.Credentials);
                    foreach (UpdateRecordStatus recordStatus in request.UpdateRecords)
                    {
                        CheckModule(recordStatus.Module);
                        CheckReportingPoint(recordStatus.Location);
                        int recordId = (int) recordStatus.MergeCriteria.SetId;
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
                    return new UpdateRecordStatusResponse {UpdateRecordStatusResults = results.ToArray()};
                });
        }

        /// <summary>
        /// Gets the views.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public GetViewsResponse GetViews(GetViewsRequest request)
        {
            return TryCatchThrowFault(() =>
                {
                    CheckModule(request.Module);
                    CheckLocation(request.ViewPoint);
                    GetViewsResponse response = new GetViewsResponse
                        {
                            Views = new[]
                                {
                                    GetViewFunc()
                                }
                        };
                    return response;
                });
        }
        
        public Func<GetView> GetViewFunc
        {
            get; set;
        }

        /// <summary>
        /// Splits the records.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SplitRecordsResponse SplitRecords(SplitRecordsRequest request)
        {
            return TryCatchThrowFault(() =>
                {
                    CheckModule(request.OriginalRecord.Module);
                    CheckReportingPoint(request.OriginalRecord.Location);

                    InMemoryRecord record = FindRecord(request.OriginalRecord.Location, request.OriginalRecord.Module,
                                                       (int) request.OriginalRecord.SetId);
                    InMemoryRecord[] splitRecords = record.SplitRecord(request.SplitRecords[0].SplitDateTimeUtc);

                    splitRecords[1].RecordId = ++setId;

                    database[splitRecords[0].RecordId] = splitRecords[0];
                    database[splitRecords[1].RecordId] = splitRecords[1];

                    return new SplitRecordsResponse
                        {
                            Context = new SplitRecordsResponseContext {OriginalRecord = request.OriginalRecord},
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
                });
        }

        private DataSubmissionResult InsertDataRecord(SubmitDataRecord submitDataRecord)
        {
            setId++;

            InMemoryRecord amplaRecord = new InMemoryRecord { Location = submitDataRecord.Location, Module = module, RecordId = setId };

            foreach (Field field in submitDataRecord.Fields)
            {
                amplaRecord.Fields.Add(new FieldValue(field.Name, field.Value));
            }

            AddDefaultFields(amplaRecord);

            database[setId] = amplaRecord;
            return new DataSubmissionResult
            {
                RecordAction = RecordAction.Insert,
                SetId = setId
            };
        }

        private void AddDefaultFields(InMemoryRecord amplaRecord)
        {
            AddDefault(amplaRecord, "CreatedDateTime", DateTime.Now);
            AddDefault(amplaRecord, "CreatedBy", userName);
        }

        private void AddDefault<T>(InMemoryRecord amplaRecord, string field, T defaultValue)
        {
            if (amplaRecord.Find(field) == null)
            {
                amplaRecord.SetFieldValue(field, defaultValue);
            }
        }


        private DataSubmissionResult UpdateDataRecord(SubmitDataRecord submitDataRecord)
        {
            int recordId = (int)submitDataRecord.MergeCriteria.SetId;

            InMemoryRecord record = FindRecord(submitDataRecord.Location, submitDataRecord.Module, recordId);
            DateTime editTime = DateTime.UtcNow;
            foreach (Field field in submitDataRecord.Fields)
            {
                string oldValue = string.Empty;

                FieldValue fieldValue = record.Find(field.Name);
                if (fieldValue == null)
                {
                    record.Fields.Add(new FieldValue(field.Name, field.Value));
                }
                else
                {
                    oldValue = fieldValue.Value;
                    fieldValue.Value = field.Value;
                }
                AddAuditRecord(record, editTime, field.Name, oldValue, field.Value);
            }

            return new DataSubmissionResult
            {
                RecordAction = RecordAction.Update,
                SetId = recordId
            };
        }

        private void AddAuditRecord(InMemoryRecord record, DateTime editedTime, string displayName, string oldValue, string newValue)
        {
            InMemoryAuditRecord auditRecord = new InMemoryAuditRecord
                {
                    SetId = Convert.ToString(record.RecordId),
                    Location = record.Location,
                    RecordType = record.Module,
                    EditedBy = "System Configuration.Users." + userName,
                    EditedDateTime = PersistenceHelper.ConvertToString(editedTime),
                    Field = GetFieldNameFromDisplayName(displayName),
                    OriginalValue = oldValue,
                    EditedValue = newValue
                };

            auditDatabase.Add(auditRecord);
        }

        private string GetFieldNameFromDisplayName(string displayName)
        {
            GetView view = GetViewFunc();
            foreach (GetViewsField field in view.Fields)
            {
                if (field.displayName == displayName)
                {
                    return field.name;
                }
            }
            return displayName;
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
                string message = string.Format("Invalid Credentials (User:'{0}', Password:'{1}', Session:'{2}')",
                                               credentials.Username, credentials.Password, credentials.Session);
                throw new ArgumentException(message, "credentials");
            }
        }
        
        private void CheckModule(AmplaModules checkModule)
        {
            if (checkModule != amplaModule)
            {
                throw new ArgumentException("Invalid module: " + checkModule);
            }
        }

        private void CheckLocation(string location)
        {
            bool isValid = reportingPoints.Any(point => point == location)
                || reportingPoints.Any(point => point.StartsWith(location)); 

            if (!isValid)
            {
                throw new ArgumentException("Invalid location: '" + location + "'.");
            }
        }

        private void CheckReportingPoint(string location)
        {
            bool isValid = reportingPoints.Any(point => point == location);

            if (!isValid)
            {
                throw new ArgumentException("Invalid location: '" + location + "'.");
            }
        }

        public List<InMemoryRecord> DatabaseRecords
        {
            get
            {
                return new List<InMemoryRecord>(database.Values);
            }
        }

        public int AddExistingRecord(InMemoryRecord record)
        {
            setId++;

            InMemoryRecord clone = record.Clone();
            clone.RecordId = setId;

            database[setId] = clone;
            return setId;
        }

        public Credentials CreateCredentials()
        {
            return new Credentials { Username = userName, Password = password };
        }

        private TResult TryCatchThrowFault<TResult>(Func<TResult> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.ToString());
            }
        }
    }
}