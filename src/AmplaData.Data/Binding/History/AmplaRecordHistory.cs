using System.Collections.Generic;
using AmplaData.Data.Binding.ViewData;
using AmplaData.Data.Records;

namespace AmplaData.Data.Binding.History
{
    public class AmplaRecordHistory<TModel>
    {
        private readonly AmplaRecord amplaRecord;
        private readonly AmplaAuditRecord auditRecord;
        private readonly IAmplaViewProperties<TModel> viewProperties;

        public AmplaRecordHistory(AmplaRecord amplaRecord, AmplaAuditRecord auditRecord, IAmplaViewProperties<TModel> viewProperties )
        {
            this.amplaRecord = amplaRecord;
            this.auditRecord = auditRecord;
            this.viewProperties = viewProperties;
        }

        public List<AmplaRecordChanges> Reconstruct()
        {
            List<AmplaRecordChanges> changes = new List<AmplaRecordChanges>();

            List<RecordEventDectection> detectors = new List<RecordEventDectection>
                {
                    new CreateRecordEventDectection<TModel>(amplaRecord, auditRecord, viewProperties),
                    new ModifyRecordEventDectection<TModel>(amplaRecord, auditRecord, viewProperties),
                    new ConfirmRecordEventDectection(),
                    new UnconfirmRecordEventDectection(),
                    new DeleteRecordEventDectection<TModel>(amplaRecord, auditRecord, viewProperties),
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