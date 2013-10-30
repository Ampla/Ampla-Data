using System.Collections.Generic;
using AmplaWeb.Data.Records;

namespace AmplaWeb.Data.Binding.History
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