using System;
using System.Collections.Generic;
using AmplaData.Data.Binding.ViewData;
using AmplaData.Data.Records;

namespace AmplaData.Data.Binding.History
{
    public class CreateRecordEventDectection<TModel> : RecordEventDectection
    {
        private readonly AmplaRecord amplaRecord;
        private readonly AmplaAuditRecord auditRecord;
        private readonly IAmplaViewProperties<TModel> viewProperties;

        private const string systemConfigurationUsers = "System Configuration.Users.";

        public CreateRecordEventDectection(AmplaRecord amplaRecord, AmplaAuditRecord auditRecord, IAmplaViewProperties<TModel> viewProperties) : base("Create Record")
        {
            this.amplaRecord = amplaRecord;
            this.auditRecord = auditRecord;
            this.viewProperties = viewProperties;
        }

        public override List<AmplaRecordChanges> DetectChanges()
        {
            DateTime createdDate = amplaRecord.GetValueOrDefault("CreatedDateTime", DateTime.MinValue);
            string user = amplaRecord.GetValueOrDefault("CreatedBy", "System");
            DateTime samplePeriod = amplaRecord.GetValueOrDefault("Sample Period", DateTime.MinValue);
            DateTime startTime = amplaRecord.GetValueOrDefault("Start Time", DateTime.MinValue);

            if (user.StartsWith(systemConfigurationUsers))
            {
                user = user.Substring(systemConfigurationUsers.Length);
            }

            List<AmplaRecordChanges> recordChanges = new List<AmplaRecordChanges>();

            DateTime recordTime =       createdDate != DateTime.MinValue ? createdDate 
                                      : samplePeriod != DateTime.MinValue ? samplePeriod
                                      : startTime != DateTime.MinValue ? startTime
                                      : DateTime.Now;

            recordChanges.Add(new AmplaRecordChanges
            {
                Operation = Operation,
                User = user,
                VersionDateTime = recordTime,
                Changes = new AmplaAuditField[0],
                Display = string.Format("{0} created record", user)                
            });

            return recordChanges;
        }
    }
}