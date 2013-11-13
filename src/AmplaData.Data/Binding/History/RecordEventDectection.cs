using System.Collections.Generic;
using AmplaData.Data.Records;

namespace AmplaData.Data.Binding.History
{
    public abstract class RecordEventDectection
    {

        protected RecordEventDectection(string operation)
        {
            Operation = operation;
        }

        protected string Operation { get; private set; }

        public abstract List<AmplaRecordChanges> DetectChanges();
    }
}