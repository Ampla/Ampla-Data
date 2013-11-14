using System.Collections.Generic;
using AmplaData.Records;

namespace AmplaData.Binding.History
{
    public class ConfirmRecordEventDectection : RecordEventDectection
    {
        public ConfirmRecordEventDectection() : base("Confirm Record")
        {
        }

        public override List<AmplaRecordChanges> DetectChanges()
        {
            List<AmplaRecordChanges> recordChanges = new List<AmplaRecordChanges>();
            return recordChanges;
        }
    }
}