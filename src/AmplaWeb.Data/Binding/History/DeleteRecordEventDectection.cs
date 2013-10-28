using System.Collections.Generic;
using AmplaWeb.Data.Records;

namespace AmplaWeb.Data.Binding.History
{
    public class DeleteRecordEventDectection : RecordEventDectection
    {
        public DeleteRecordEventDectection() : base("Delete Record")
        {
        }

        public override List<AmplaRecordChanges> DetectChanges()
        {
            List<AmplaRecordChanges> recordChanges = new List<AmplaRecordChanges>();
            return recordChanges;
        }
    }
}