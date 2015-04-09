using System.Collections.Generic;

namespace AmplaData.Records
{
    public interface IAmplaDatabase
    {
        Dictionary<int, InMemoryRecord> GetModuleRecords(string module);
        int GetNewSetId(string module);
        List<InMemoryAuditRecord> GetAuditRecords(string module);
    }
}