using System.Collections.Generic;
using AmplaData.Records;

namespace AmplaData.Binding.History
{
    public class UnconfirmRecordEventDectection : RecordEventDectection
    {
        public UnconfirmRecordEventDectection() : base("Unconfirm record")
        {
        }

        public override List<AmplaRecordChanges> DetectChanges()
        {
            List<AmplaRecordChanges> recordChanges = new List<AmplaRecordChanges>();
            return recordChanges;
        }
    }
}