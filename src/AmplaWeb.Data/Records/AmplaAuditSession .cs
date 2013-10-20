using System;
using System.Collections.Generic;

namespace AmplaWeb.Data.Records
{
    public class AmplaAuditSession 
    {
        public AmplaAuditSession(string user, DateTime editedTime)
        {
            User = user;
            EditedTime = editedTime;
            Fields = new List<AmplaAuditField>();
        }

        public string User { get; private set; }

        public DateTime EditedTime { get; set; }

        public List<AmplaAuditField> Fields { get; private set; }
    }
}