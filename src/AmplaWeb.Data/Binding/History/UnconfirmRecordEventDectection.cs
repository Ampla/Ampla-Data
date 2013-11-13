using System;
using System.Collections.Generic;
using AmplaData.Data.Records;

namespace AmplaData.Data.Binding.History
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