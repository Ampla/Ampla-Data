using System;
using System.Collections.Generic;
using AmplaData.Data.Binding.History;
using AmplaData.Data.Binding.ModelData;
using AmplaData.Data.Binding.ViewData;
using AmplaData.Data.Records;

namespace AmplaData.Data.Binding
{
    public class AmplaGetDataVersionsBinding<TModel> : IAmplaBinding where TModel : class, new()
    {
        private readonly AmplaRecord amplaRecord;
        private readonly AmplaAuditRecord auditRecord;
        private readonly TModel currentModel;
        private readonly ModelVersions versions;
        private readonly IModelProperties<TModel> modelProperties;
        private readonly IAmplaViewProperties<TModel> viewProperties;

        public AmplaGetDataVersionsBinding(AmplaRecord amplaRecord, AmplaAuditRecord auditRecord, TModel currentModel, ModelVersions versions, IModelProperties<TModel> modelProperties, IAmplaViewProperties<TModel> viewProperties)
        {
            this.amplaRecord = amplaRecord;
            this.auditRecord = auditRecord;
            this.currentModel = currentModel;
            this.versions = versions;
            this.modelProperties = modelProperties;
            this.viewProperties = viewProperties;
        }

        public bool Bind()
        {
            versions.ModelName = modelProperties.GetModelName();

            AmplaRecordHistory<TModel> history = new AmplaRecordHistory<TModel>(amplaRecord, auditRecord, viewProperties);
            List<AmplaRecordChanges> recordChanges = history.Reconstruct();

            ModelVersion<TModel>[] modelVersions = new ModelVersion<TModel>[recordChanges.Count];

            TModel clonedModel = modelProperties.CloneModel(currentModel);

            for (int i = 0; i < modelVersions.Length; i++)
            {
                AmplaRecordChanges recordChange = recordChanges[i];
                bool isCurrent = i == (modelVersions.Length - 1);
                modelVersions[i] = new ModelVersion<TModel>(isCurrent, clonedModel)
                    {
                        Version = i + 1,
                        User = recordChange.User,
                        VersionDate = recordChange.VersionDateTime,
                        Display = recordChange.Display
                    };
            }

            TModel current = currentModel;
            for (int i = modelVersions.Length - 1; i >= 0; i--)
            {
                var modelVersion = modelVersions[i];
                modelVersion.Model = modelProperties.CloneModel(current);
                AmplaRecordChanges recordChange = recordChanges[i];

                foreach (var field in recordChange.Changes)
                {
                    viewProperties.UpdateModel(current, field.Name, field.OriginalValue, false);
                }
                if (i == 0)
                {
                    modelVersion.Model = modelProperties.CloneModel(current);
                }
            }

            foreach (var modelVersion in modelVersions)
            {
                versions.Versions.Add(modelVersion);
            }
            return true;
        }

        public bool Validate()
        {
            return auditRecord != null && (!Equals(currentModel, null));
        }
    }
}