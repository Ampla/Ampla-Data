using System.Collections.Generic;
using AmplaData.Records;

namespace AmplaData.Database
{
    public interface IAmplaDatabase
    {
        Dictionary<int, InMemoryRecord> GetModuleRecords(string module);
        int GetNewSetId(string module);
        List<InMemoryAuditRecord> GetAuditRecords(string module);
    }
}