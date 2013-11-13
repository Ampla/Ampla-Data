using System;
using System.Collections.Generic;
using AmplaData.Data.AmplaData2008;
using AmplaData.Data.Attributes;
using AmplaData.Data.Binding.ModelData;
using AmplaData.Data.Binding.ViewData;
using AmplaData.Data.Production;
using AmplaData.Data.Records;
using NUnit.Framework;

namespace AmplaData.Data.Binding
{
    [TestFixture]
    public class AmplaGetDataVersionsBindingUnitTests : TestFixture
    {
        [AmplaLocation(Location = location)]
        [AmplaModule(Module = module)]
        private class AreaValueModel
        {
            public int Id { get; set; }
            public double Value { get; set; }
            public string Area { get; set; }
            public bool Confirmed { get; set; }
        }

        private const string module = "Production";
        private const string location = "Enterprise.Site.Area.Production";

        /// <summary>
        /// Issue 13 - Repository.GetVersions() will throw exceptions with empty values in the AuditLog
        /// https://github.com/Ampla/Ampla-Data-MVC/issues/13
        /// </summary>
        [Test]
        public void BindWithEmptyOriginalValue_double()
        {
            AmplaRecord amplaRecord = new AmplaRecord(100) {Location = location, Module = module, ModelName = ""};
            amplaRecord.AddColumn("Value", typeof (double));
            amplaRecord.AddColumn("Area", typeof (string));
            amplaRecord.SetValue("Value", "100.0");
            amplaRecord.SetValue("Area", "ROM");

            AmplaAuditRecord auditRecord = new AmplaAuditRecord
                {
                    Id = amplaRecord.Id,
                    Location = amplaRecord.Location,
                    Module = amplaRecord.Module,
                    Changes = new List<AmplaAuditSession>
                        {
                            new AmplaAuditSession("User", DateTime.Today)
                        }
                };
            auditRecord.Changes[0].Fields.Add(new AmplaAuditField
                {
                    Name = "Value",
                    OriginalValue = "",
                    EditedValue = "100"
                });

            AreaValueModel model = new AreaValueModel {Id = 100, Area = "ROM", Value = 100.0d};
            ModelVersions modelVersions = new ModelVersions(amplaRecord);
            IModelProperties<AreaValueModel> modelProperties = new ModelProperties<AreaValueModel>();

            AmplaViewProperties<AreaValueModel> viewProperties = new AmplaViewProperties<AreaValueModel>(modelProperties);
            GetViewsResponse view = new GetViewsResponse
                {
                    Context = new GetViewsResponseContext(),
                    Views = new[] {ProductionViews.AreaValueModelView()}
                };
            viewProperties.Initialise(view);
            AmplaGetDataVersionsBinding<AreaValueModel> binding =
                new AmplaGetDataVersionsBinding<AreaValueModel>(amplaRecord, auditRecord, model, modelVersions,
                                                                modelProperties, viewProperties);
            Assert.That(binding.Validate(), Is.True);
            Assert.That(binding.Bind(), Is.True);

            Assert.That(modelVersions.Versions, Is.Not.Empty);
            Assert.That(modelVersions.Versions.Count, Is.EqualTo(2));

            ModelVersion<AreaValueModel> last = (ModelVersion<AreaValueModel>) modelVersions.Versions[0];
            ModelVersion<AreaValueModel> current = (ModelVersion<AreaValueModel>) modelVersions.Versions[1];

            Assert.That(last.IsCurrentVersion, Is.False);
            Assert.That(current.IsCurrentVersion, Is.True);

            Assert.That(last.Model.Area, Is.EqualTo(model.Area));
            Assert.That(current.Model.Area, Is.EqualTo(model.Area));

            Assert.That(last.Model.Value, Is.EqualTo(0d));
            Assert.That(current.Model.Value, Is.EqualTo(100d));

            Assert.That(last.Model.Id, Is.EqualTo(100));
            Assert.That(current.Model.Id, Is.EqualTo(100));
        }

        /// <summary>
        /// Issue 13 - Repository.GetVersions() will throw exceptions with empty values in the AuditLog
        /// https://github.com/Ampla/Ampla-Data-MVC/issues/13
        /// </summary>
        [Test]
        public void BindWithEmptyOriginalValue_bool()
        {
            AmplaRecord amplaRecord = new AmplaRecord(100) { Location = location, Module = module, ModelName = "" };
            amplaRecord.AddColumn("Value", typeof(double));
            amplaRecord.AddColumn("Area", typeof(string));
            amplaRecord.AddColumn("Deleted", typeof(bool));
            amplaRecord.SetValue("Value", "100.0");
            amplaRecord.SetValue("Area", "ROM");
            amplaRecord.SetValue("Deleted", "True");

            AmplaAuditRecord auditRecord = new AmplaAuditRecord
            {
                Id = amplaRecord.Id,
                Location = amplaRecord.Location,
                Module = amplaRecord.Module,
                Changes = new List<AmplaAuditSession>
                        {
                            new AmplaAuditSession("User", DateTime.Today)
                        }
            };
            auditRecord.Changes[0].Fields.Add(new AmplaAuditField
            {
                Name = "IsConfirmed",
                OriginalValue = "",
                EditedValue = "True"
            });

            AreaValueModel model = new AreaValueModel { Id = 100, Area = "ROM", Value = 100.0d, Confirmed = true};
            ModelVersions modelVersions = new ModelVersions(amplaRecord);
            IModelProperties<AreaValueModel> modelProperties = new ModelProperties<AreaValueModel>();

            AmplaViewProperties<AreaValueModel> viewProperties = new AmplaViewProperties<AreaValueModel>(modelProperties);
            GetViewsResponse view = new GetViewsResponse
            {
                Context = new GetViewsResponseContext(),
                Views = new[] { ProductionViews.AreaValueModelView() }
            };
            viewProperties.Initialise(view);
            AmplaGetDataVersionsBinding<AreaValueModel> binding =
                new AmplaGetDataVersionsBinding<AreaValueModel>(amplaRecord, auditRecord, model, modelVersions,
                                                                modelProperties, viewProperties);
            Assert.That(binding.Validate(), Is.True);
            Assert.That(binding.Bind(), Is.True);

            Assert.That(modelVersions.Versions, Is.Not.Empty);
            Assert.That(modelVersions.Versions.Count, Is.EqualTo(2));

            ModelVersion<AreaValueModel> last = (ModelVersion<AreaValueModel>)modelVersions.Versions[0];
            ModelVersion<AreaValueModel> current = (ModelVersion<AreaValueModel>)modelVersions.Versions[1];

            Assert.That(last.IsCurrentVersion, Is.False);
            Assert.That(current.IsCurrentVersion, Is.True);

            Assert.That(last.Model.Area, Is.EqualTo(model.Area));
            Assert.That(current.Model.Area, Is.EqualTo(model.Area));

            Assert.That(last.Model.Value, Is.EqualTo(100d));
            Assert.That(current.Model.Value, Is.EqualTo(100d));

            Assert.That(last.Model.Id, Is.EqualTo(100));
            Assert.That(current.Model.Id, Is.EqualTo(100));

            Assert.That(last.Model.Confirmed, Is.EqualTo(false));
            Assert.That(current.Model.Confirmed, Is.EqualTo(true));
        }
    }
}