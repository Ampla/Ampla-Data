using System.Collections.Generic;
using System.Linq;
using AmplaWeb.Data.Records;

namespace AmplaWeb.Data.Binding.History
{
    public class DeleteRecordEventDectection : RecordEventDectection
    {
        private readonly AmplaRecord amplaRecord;
        private readonly AmplaAuditRecord amplaAuditRecord;

        private const string deletedDisplayName = "Deleted";
        private const string deletedName = "IsDeleted";

        public DeleteRecordEventDectection(AmplaRecord amplaRecord, AmplaAuditRecord amplaAuditRecord) : base("Delete Record")
        {
            this.amplaRecord = amplaRecord;
            this.amplaAuditRecord = amplaAuditRecord;
        }

        public override List<AmplaRecordChanges> DetectChanges()
        {
            List<AmplaRecordChanges> recordChanges = new List<AmplaRecordChanges>();
            if (amplaRecord.GetValueOrDefault(deletedDisplayName, false))
            {
                if (amplaAuditRecord.Changes != null)
                {
                    foreach (AmplaAuditSession session in amplaAuditRecord.Changes)
                    {
                        AmplaAuditField isDeleted = session.Fields.FirstOrDefault(f => f.Name == deletedName);
                        if (isDeleted != null)
                        {
                            AmplaRecordChanges changes = new AmplaRecordChanges
                            {
                                VersionDateTime = session.EditedTime,
                                User = session.User,
                                Operation = Operation,
                                Changes = new AmplaAuditField[0],
                                Display = string.Format("{0} deleted record", session.User)
                            };
                            recordChanges.Add(changes);
                        }
                    }
                }                
            }
            return recordChanges;
        }
    }
}