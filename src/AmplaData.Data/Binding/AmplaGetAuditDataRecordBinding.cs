using System;
using System.Collections.Generic;
using AmplaData.Data.AmplaData2008;
using AmplaData.Data.Binding.MetaData;
using AmplaData.Data.Binding.ModelData;
using AmplaData.Data.Records;

namespace AmplaData.Data.Binding
{
    public class AmplaGetAuditDataRecordBinding<TModel> : IAmplaBinding where TModel : new()
    {
        private readonly GetAuditDataResponse response;
        private readonly AmplaRecord record;
        private readonly List<AmplaAuditRecord> records;
        private readonly IModelProperties<TModel> modelProperties;

        private const string systemConfigUsers = "System Configuration.Users.";

        public AmplaGetAuditDataRecordBinding(GetAuditDataResponse response, AmplaRecord record, List<AmplaAuditRecord> records, IModelProperties<TModel> modelProperties)
        {
            this.response = response;
            this.record = record;
            this.records = records;
            this.modelProperties = modelProperties;
        }

        public bool Bind()
        {
            if (response.RowSets.Length == 0) return false;

            GetAuditDataRowSet rowSet = response.RowSets[0];

            AmplaAuditRecord model = new AmplaAuditRecord {Id = record.Id, Location = record.Location, Module = record.Module };

            List<AmplaAuditSession> changes = new List<AmplaAuditSession>();

            foreach (GetAuditDataRow row in rowSet.Rows)
            {
                if (row.Field != "LastModified")
                {
                    DateTime editTime = Iso8601DateTimeConverter.ConvertToLocalDateTime(row.EditedDateTime);
                    string user = row.EditedBy.StartsWith(systemConfigUsers)
                                      ? row.EditedBy.Substring(systemConfigUsers.Length)
                                      : row.EditedBy;
                    AmplaAuditSession session = changes.Find(s => s.EditedTime == editTime && s.User == user);
                    if (session == null)
                    {
                        session = new AmplaAuditSession(user, editTime);
                        changes.Add(session);
                    }

                    session.Fields.Add(new AmplaAuditField
                        {
                            Name = row.Field,
                            OriginalValue = row.OriginalValue,
                            EditedValue = row.EditedValue
                        });
                }
            }

            changes.Sort();

            model.Changes = changes;
            records.Add(model);
            return true;
        }

        public bool Validate()
        {
            return true;
        }
    }
}