using System;
using System.Collections.Generic;
using AmplaWeb.Data.Binding.ModelData;
using AmplaWeb.Data.Binding.ViewData;
using AmplaWeb.Data.Records;

namespace AmplaWeb.Data.Binding
{
    public class AmplaGetDataVersionsBinding<TModel> : IAmplaBinding where TModel : new()
    {
        private readonly AmplaAuditRecord auditRecord;
        private readonly TModel currentModel;
        private readonly List<ModelVersion> versions;
        private readonly IModelProperties<TModel> modelProperties;
        private IAmplaViewProperties viewProperties;

        public AmplaGetDataVersionsBinding(AmplaAuditRecord auditRecord, TModel currentModel, List<ModelVersion> versions, IModelProperties<TModel> modelProperties, IAmplaViewProperties viewProperties)
        {
            this.auditRecord = auditRecord;
            this.currentModel = currentModel;
            this.versions = versions;
            this.modelProperties = modelProperties;
            this.viewProperties = viewProperties;
        }

        public bool Bind()
        {
            ModelVersion<TModel> current = new ModelVersion<TModel>(true, currentModel);
            TModel model = modelProperties.CloneModel(currentModel);

            List<string> mappedFields = new List<string>();
            mappedFields.AddRange(modelProperties.GetProperties());
            for (int i = auditRecord.Changes.Count - 1; i >= 0; i--)
            {
                bool newVersion = false;
                AmplaAuditSession session = auditRecord.Changes[i];
                foreach (AmplaAuditField auditField in session.Fields)
                {
                    if (mappedFields.Contains(auditField.Name))
                    {
                        string currentValue;
                        if (modelProperties.TryGetPropertyValue(model, auditField.Name, out currentValue))
                        {
                            if (StringComparer.InvariantCulture.Compare(currentValue, auditField.EditedValue) == 0)
                            {
                                if (modelProperties.TrySetValueFromString(model, auditField.Name,
                                                                          auditField.OriginalValue))
                                {
                                    newVersion = true;
                                }
                            }
                        }
                    }
                }
                if (newVersion)
                {
                    ModelVersion<TModel> modelVersion = new ModelVersion<TModel>(false, model);
                    versions.Add(modelVersion);
                    model = modelProperties.CloneModel(model);
                }
            }

            versions.Add(current);
            return true;
        }

        public bool Validate()
        {
            return auditRecord != null && (!Equals(currentModel, null));
        }
    }
}