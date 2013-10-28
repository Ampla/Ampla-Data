using System.Collections.Generic;
using System.Linq;
using AmplaWeb.Data.Records;

namespace AmplaWeb.Data.Binding.History
{
    public class ModifyRecordEventDectection : RecordEventDectection
    {
        private readonly AmplaRecord amplaRecord;
        private readonly AmplaAuditRecord amplaAuditRecord;

        private readonly List<string> ignoreFields = new List<string> {"IsDeleted"}; 

        public ModifyRecordEventDectection(AmplaRecord amplaRecord, AmplaAuditRecord amplaAuditRecord) : base("Modify Record")
        {
            this.amplaRecord = amplaRecord;
            this.amplaAuditRecord = amplaAuditRecord;
        }

        public override List<AmplaRecordChanges> DetectChanges()
        {
            List<AmplaRecordChanges> recordChanges = new List<AmplaRecordChanges>();
            if (amplaAuditRecord.Changes != null)
            {
                foreach (AmplaAuditSession session in amplaAuditRecord.Changes)
                {
                    List<AmplaAuditField> fields = session.Fields.Where(IncludeField).ToList();
                    if (fields.Count > 0)
                    {
                        string fieldNames = string.Join(", ", fields.Select(field => field.Name));
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