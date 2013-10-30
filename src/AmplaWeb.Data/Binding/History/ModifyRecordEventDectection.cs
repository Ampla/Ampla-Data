using System.Collections.Generic;
using System.Linq;
using AmplaWeb.Data.Binding.ViewData;
using AmplaWeb.Data.Records;

namespace AmplaWeb.Data.Binding.History
{
    public class ModifyRecordEventDectection<TModel> : RecordEventDectection
    {
        private readonly AmplaRecord amplaRecord;
        private readonly AmplaAuditRecord amplaAuditRecord;
        private readonly IAmplaViewProperties<TModel> viewProperties;

        private readonly List<string> ignoreFields = new List<string> {"IsDeleted"};

        public ModifyRecordEventDectection(AmplaRecord amplaRecord, AmplaAuditRecord amplaAuditRecord, IAmplaViewProperties<TModel> viewProperties)
            : base("Modify Record")
        {
            this.amplaRecord = amplaRecord;
            this.amplaAuditRecord = amplaAuditRecord;
            this.viewProperties = viewProperties;
        }

        public override List<AmplaRecordChanges> DetectChanges()
        {
            List<AmplaRecordChanges> recordChanges = new List<AmplaRecordChanges>();
            if (amplaAuditRecord.Changes != null)
            {
                foreach (AmplaAuditSession session in amplaAuditRecord.Changes)
                {
                    List<string> fieldList = new List<string>();
                    List<AmplaAuditField> fields = new List<AmplaAuditField>();
                    foreach (var auditField in session.Fields.Where(IncludeField))
                    {
                        fields.Add(auditField);
                        fieldList.Add(viewProperties.GetFieldDisplayName(auditField.Name));
                    }
                    if (fields.Count > 0)
                    {
                        string fieldNames = string.Join(", ", fieldList);
                        AmplaRecordChanges changes = new AmplaRecordChanges
                            {
                                VersionDateTime = session.EditedTime,
                                User = session.User,
                                Operation = Operation,
                                Changes = fields.ToArray(),
                                Display = string.Format("{0} modified record ({1})", session.User, fieldNames)
                            };
                        recordChanges.Add(changes);
                    }
                }
            }
            return recordChanges;
        }

        private bool IncludeField(AmplaAuditField field)
        {
            return (field.OriginalValue != field.EditedValue)
                   && !ignoreFields.Contains(field.Name);
        }
    }
}