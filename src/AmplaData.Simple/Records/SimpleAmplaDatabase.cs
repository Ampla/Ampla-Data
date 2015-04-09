using System;
using System.Collections.Generic;

namespace AmplaData.Records
{
    public class SimpleAmplaDatabase : IAmplaDatabase
    {
        private readonly Dictionary<string, Dictionary<int, InMemoryRecord>> recordsByModule = new Dictionary<string, Dictionary<int, InMemoryRecord>>();
        private readonly Dictionary<string, int> setIdsByModule = new Dictionary<string, int>();
        private readonly Dictionary<string, List<InMemoryAuditRecord>> auditRecordsByModule = new Dictionary<string, List<InMemoryAuditRecord>>();
        
        public void EnableModule(string module)
        {
            recordsByModule.Add(module, new Dictionary<int, InMemoryRecord>());
            setIdsByModule.Add(module, (setIdsByModule.Count + 1) * 1000);
            auditRecordsByModule.Add(module, new List<InMemoryAuditRecord>());
        }

        public Dictionary<int, InMemoryRecord> GetModuleRecords(string module)
        {
            Dictionary<int, InMemoryRecord> moduleRecords;
            if (recordsByModule.TryGetValue(module, out moduleRecords))
            {
                return moduleRecords;
            }

            throw new ArgumentException("Invalid Module: " + module);
        }

        public int GetNewSetId(string module)
        {
            int setId;
            if (setIdsByModule.TryGetValue(module, out setId))
            {
                int newSetId = setId + 1;
                setIdsByModule[module] = newSetId;
                return newSetId;
            }
            throw new ArgumentException("Invalid Module: " + module);
        }

        public List<InMemoryAuditRecord> GetAuditRecords(string module)
        {
            List<InMemoryAuditRecord> auditRecords;
            if (auditRecordsByModule.TryGetValue(module, out auditRecords))
            {
                return auditRecords;
            }

            throw new ArgumentException("Invalid Module: " + module);
        }
    }
}