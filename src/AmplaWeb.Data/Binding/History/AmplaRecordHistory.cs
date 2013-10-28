using System.Collections.Generic;
using AmplaWeb.Data.Records;

namespace AmplaWeb.Data.Binding.History
{
    public class AmplaRecordHistory
    {
        private readonly AmplaRecord amplaRecord;
        private readonly AmplaAuditRecord auditRecord;

        public AmplaRecordHistory(AmplaRecord amplaRecord, AmplaAuditRecord auditRecord)
        {
            this.amplaRecord = amplaRecord;
            this.auditRecord = auditRecord;
        }

        public List<AmplaRecordChanges> Reconstruct()
        {
            List<AmplaRecordChanges> changes = new List<AmplaRecordChanges>();

            List<RecordEventDectection> detectors = new List<RecordEventDectection>
                {
                    new CreateRecordEventDectection(amplaRecord, auditRecord),
                    new ModifyRecordEventDectection(amplaRecord, auditRecord),
                    new ConfirmRecordEventDectection(),
                    new UnconfirmRecordEventDectection(),
                    new DeleteRecordEventDectection(),
                };

            foreach (RecordEventDectection detector in detectors)
            {
                changes.AddRange(detector.DetectChanges());
            }

            changes.Sort();

            return changes;
        }
    }
}