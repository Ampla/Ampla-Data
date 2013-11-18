using System;
using System.Collections.Generic;
using AmplaData.Attributes;
using AmplaData.Binding.ModelData;
using AmplaData.Binding.ViewData;
using AmplaData.Records;
using NUnit.Framework;

namespace AmplaData.Binding.History
{
    [TestFixture]
    public abstract class RecordEventDetectionTestFixture : TestFixture
    {
        private const string location = "Enterprise.Site.Area.Production";
        private const string module = "Production";

        private IModelProperties<ProductionModel> modelProperties;
        private IAmplaViewProperties<ProductionModel> amplaViewProperties;

        [AmplaLocation(Location = location)]
        [AmplaModule(Module = module)]
        protected class ProductionModel
        {
            public int Id { get; set; }
        }

        protected override void OnFixtureSetUp()
        {
            base.OnFixtureSetUp();
            modelProperties = new ModelProperties<ProductionModel>();
            amplaViewProperties = new AmplaViewProperties<ProductionModel>(modelProperties); 
        }

        protected AmplaRecord CreateRecord(int id)
        {
            AmplaRecord record = new AmplaRecord(id)
                {
                    Location = location,
                    Module = module,
                    ModelName = "Production Model"
                };

            return record;
        }

        protected AmplaAuditRecord CreateAuditRecord(AmplaRecord record)
        {
            AmplaAuditRecord auditRecord = new AmplaAuditRecord
                {
                    Id = record.Id,
                    Location = record.Location,
                    Module = record.Module,
                    Changes = new List<AmplaAuditSession>()
                };
            return auditRecord;
        }

        protected IAmplaViewProperties<ProductionModel> GetViewProperties()
        {
            return amplaViewProperties;
        }

        protected static AmplaAuditSession AddSession(string user, DateTime time, string[] fields, string[] oldValues,
                                      string[] newValues)
        {
            AmplaAuditSession session = new AmplaAuditSession(user, time);

            Assert.That(fields.Length, Is.GreaterThan(0));
            Assert.That(fields.Length, Is.EqualTo(oldValues.Length));
            Assert.That(fields.Length, Is.EqualTo(newValues.Length));
            for (int i = 0; i < fields.Length; i++)
            {
                AmplaAuditField field = new AmplaAuditField
                {
                    Name = fields[i],
                    OriginalValue = oldValues[i],
                    EditedValue = newValues[i]
                };
                session.Fields.Add(field);
            }
            return session;
        }

        protected static AmplaAuditSession AddSession(string user, DateTime time, string field, string oldValue,
                                             string newValue)
        {
            return AddSession(user, time, new[] { field }, new[] { oldValue }, new[] { newValue });
        }
    }
}