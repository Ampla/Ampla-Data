using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Xml;
using AmplaData.AmplaSecurity2007;
using AmplaData.Database;
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
        //private readonly string[] reportingPoints;
        private readonly SimpleSecurityWebServiceClient securityWebServiceClient;
        private readonly IAmplaDatabase amplaDatabase;
        private readonly IAmplaConfiguration amplaConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataWebServiceClient" /> class.
        /// </summary>
        /// <param name="amplaDatabase">The ampla database.</param>
        /// <param name="amplaConfiguration">The ampla configuration.</param>
        /// <param name="securityWebServiceClient">The security web service client.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">module</exception>
        public SimpleDataWebServiceClient(IAmplaDatabase amplaDatabase, IAmplaConfiguration amplaConfiguration, SimpleSecurityWebServiceClient securityWebServiceClient)
        {
            this.amplaDatabase = amplaDatabase;
            this.amplaConfiguration = amplaConfiguration;

            GetViewFunc = StandardViews.EmptyView;
            this.securityWebServiceClient = securityWebServiceClient;
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

                    CheckCredentials(request.Credentials);

                    InMemoryFilterMatcher filterMatcher = new InMemoryFilterMatcher(request.Filter);

                    List<InMemoryRecord> recordsToReturn = new List<InMemoryRecord>();

                    Dictionary<int, InMemoryRecord> database = amplaDatabase.GetModuleRecords(request.View.Module.ToString());

                    if (database.Count > 0)
                    {
                        recordsToReturn = database.Values.Where(filterMatcher.Matches).ToList();
                    }
                    bool resolveIdentifiers = request.OutputOptions != null && request.OutputOptions.ResolveIdentifiers;

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
                                    ResolveIdentifiers = resolveIdentifiers,
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
            
            InMemoryFilterMatcher filterMatcher = new InMemoryFilterMatcher(request.Filter);

            string module = request.Filter.Module.ToString();
            List<InMemoryAuditRecord> auditRecords = amplaDatabase.GetAuditRecords(module).Where(filterMatcher.Matches).ToList();

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
                    CheckCredentials(request.Credentials);

                    string module = request.Module.ToString();

                    string[] reportingPoints = amplaConfiguration.GetLocations(module);

                    SimpleNavigationHierarchy navigationHierarchy = new SimpleNavigationHierarchy(request.Module, reportingPoints);

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
                    string user = CheckCredentials(request.Credentials);
                    foreach (SubmitDataRecord submitDataRecord in request.SubmitDataRecords)
                    {
                        CheckReportingPoint(submitDataRecord.Module.ToString(), submitDataRecord.Location);
                        results.Add(submitDataRecord.MergeCriteria == null
                                        ? InsertDataRecord(submitDataRecord, user)
                                        : UpdateDataRecord(submitDataRecord, user));
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
                    DateTime editTime = DateTime.Now;
                    List<DeleteRecordsResult> results = new List<DeleteRecordsResult>();
                    string user = CheckCredentials(request.Credentials);
                    foreach (DeleteRecord deleteRecord in request.DeleteRecords)
                    {
                        Dictionary<int, InMemoryRecord> database = amplaDatabase.GetModuleRecords(deleteRecord.Module.ToString());

                        CheckReportingPoint(deleteRecord.Module.ToString(), deleteRecord.Location);
                        int recordId = (int) deleteRecord.MergeCriteria.SetId;
                        InMemoryRecord record = FindRecord(database, deleteRecord.Location, deleteRecord.Module, recordId);
                        FieldValue deleted = record.Find("Deleted");
                        if (deleted != null)
                        {
                            record.Fields.Remove(deleted);
                        }
                        record.Fields.Add(new FieldValue("Deleted", "True"));
                        AddAuditRecord(record, editTime, "IsDeleted", false.ToString(), true.ToString(), user);

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
                        string module = recordStatus.Module.ToString();
                        Dictionary<int, InMemoryRecord> database = amplaDatabase.GetModuleRecords(module);
                        CheckReportingPoint(module, recordStatus.Location);
                        int recordId = (int) recordStatus.MergeCriteria.SetId;
                        InMemoryRecord record = FindRecord(database, recordStatus.Location, recordStatus.Module, recordId);
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
                    CheckCredentials(request.Credentials);
                    string module = request.Module.ToString();
                    CheckLocation(module, request.ViewPoint);
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
                    CheckCredentials(request.Credentials);
                    string module = request.OriginalRecord.Module.ToString();

                    CheckReportingPoint(module, request.OriginalRecord.Location);
                    
                    Dictionary<int, InMemoryRecord> database = amplaDatabase.GetModuleRecords(module);

                    InMemoryRecord record = FindRecord(database, request.OriginalRecord.Location, request.OriginalRecord.Module,
                                                       (int) request.OriginalRecord.SetId);
                    InMemoryRecord[] splitRecords = record.SplitRecord(request.SplitRecords[0].SplitDateTimeUtc);

                    int newSetId = amplaDatabase.GetNewSetId(module);

                    splitRecords[1].RecordId = newSetId;

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

        private DataSubmissionResult InsertDataRecord(SubmitDataRecord submitDataRecord, string user)
        {
            string module = submitDataRecord.Module.ToString();
            Dictionary<int, InMemoryRecord> database = amplaDatabase.GetModuleRecords(module);
            int setId = amplaDatabase.GetNewSetId(module);

            setId++;
            GetView view = GetViewFunc();
            InMemoryRecord amplaRecord = new InMemoryRecord(view)
                {
                    Location = submitDataRecord.Location,
                    Module = module,
                    RecordId = setId
                };

            foreach (Field field in submitDataRecord.Fields)
            {
                amplaRecord.Fields.Add(new FieldValue(field.Name, field.Value));
            }

            AddDefaultFields(amplaRecord, user);

            database[setId] = amplaRecord;
            return new DataSubmissionResult
            {
                RecordAction = RecordAction.Insert,
                SetId = setId
            };
        }

        private void AddDefaultFields(InMemoryRecord amplaRecord, string user)
        {
            AddDefault(amplaRecord, "CreatedDateTime", DateTime.Now);
            AddDefault(amplaRecord, "CreatedBy", user);
        }

        private void AddDefault<T>(InMemoryRecord amplaRecord, string field, T defaultValue)
        {
            if (amplaRecord.Find(field) == null)
            {
                amplaRecord.SetFieldValue(field, defaultValue);
            }
        }


        private DataSubmissionResult UpdateDataRecord(SubmitDataRecord submitDataRecord, string user)
        {
            int recordId = (int)submitDataRecord.MergeCriteria.SetId;

            Dictionary<int, InMemoryRecord> database = amplaDatabase.GetModuleRecords(submitDataRecord.Module.ToString());

            InMemoryRecord record = FindRecord(database, submitDataRecord.Location, submitDataRecord.Module, recordId);
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
                AddAuditRecord(record, editTime, field.Name, oldValue, field.Value, user);
            }

            return new DataSubmissionResult
            {
                RecordAction = RecordAction.Update,
                SetId = recordId
            };
        }

        private void AddAuditRecord(InMemoryRecord record, DateTime editedTime, string displayName, string oldValue, string newValue, string user)
        {
            List<InMemoryAuditRecord> auditDatabase = amplaDatabase.GetAuditRecords(record.Module);

            InMemoryAuditRecord auditRecord = new InMemoryAuditRecord
                {
                    SetId = Convert.ToString(record.RecordId),
                    Location = record.Location,
                    RecordType = record.Module,
                    EditedBy = "System Configuration.Users." + user,
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

        private InMemoryRecord FindRecord(Dictionary<int, InMemoryRecord> database, string searchLocation, AmplaModules searchModule, int searchRecordId)
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

        private string CheckCredentials(Credentials credentials)
        {
            if (credentials == null)
            {
                throw new ArgumentException("No credentials specified.  Integrated security not enabled.", "credentials");
            }

            string userName = credentials.Username;
            string password = credentials.Password;
            string session = credentials.Session;

            if (securityWebServiceClient.ValidateUserPassword(userName, password))
            {
                return userName;
            }

            SimpleSession simpleSession = securityWebServiceClient.FindBySession(session);
            if (simpleSession != null)
            {
                return simpleSession.UserName;
            }

            string message = string.Format("Invalid Credentials (User:'{0}', Password:'{1}', Session:'{2}')",
                                           credentials.Username, credentials.Password, credentials.Session);
            throw new ArgumentException(message, "credentials");
        }

        private void CheckLocation(string module, string location)
        {
            string[] locations = amplaConfiguration.GetLocations(module);
            bool isValid = locations.Any(point => point == location)
                || locations.Any(point => point.StartsWith(location)); 

            if (!isValid)
            {
                throw new ArgumentException("Invalid location: '" + location + "'.");
            }
        }

        private void CheckReportingPoint(string module, string location)
        {
            string[] locations = amplaConfiguration.GetLocations(module);
            bool isValid = locations.Any(point => point == location);

            if (!isValid)
            {
                throw new ArgumentException("Invalid location: '" + location + "'.");
            }
        }

        public int AddExistingRecord(InMemoryRecord record)
        {
            string module = record.Module;
            int newSetId = amplaDatabase.GetNewSetId(module);
            Dictionary<int, InMemoryRecord> database = amplaDatabase.GetModuleRecords(module);

            InMemoryRecord clone = record.Clone();
            clone.RecordId = newSetId;

            database[newSetId] = clone;
            return newSetId;
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