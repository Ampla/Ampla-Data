using System;

namespace AmplaWeb.Data.Records
{
    public class AmplaAuditField 
    {
        public string Name { get; set; }
        public Type DataType { get; set; }
        public string OriginalValue { get; set; }
        public string EditedValue { get; set; }
    }
}