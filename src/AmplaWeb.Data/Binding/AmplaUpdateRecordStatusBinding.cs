using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding
{
    public abstract class AmplaUpdateRecordStatusBinding<TModel> : IAmplaBinding where TModel : new()
    {
        private readonly List<TModel> models;
        private readonly List<UpdateRecordStatus> records;
        private readonly IModelProperties<TModel> modelProperties;

        private readonly UpdateRecordStatusAction recordAction;
        private readonly string idProperty;

        protected AmplaUpdateRecordStatusBinding(List<TModel> models, List<UpdateRecordStatus> records, IModelProperties<TModel> modelProperties, UpdateRecordStatusAction recordAction)
        {
            this.models = models;
            this.records = records;
            this.modelProperties = modelProperties;
            this.recordAction = recordAction;
            idProperty = ModelIdentifier.GetPropertyName<TModel>();
        }

        public bool Bind()
        {
            if (models.Count == 0) return false;

            foreach (TModel model in models)
            {
                UpdateRecordStatus record = new UpdateRecordStatus
                {
                    Location = modelProperties.GetLocation(model),
                    Module = modelProperties.Module,

                    MergeCriteria = new UpdateRecordStatusMergeCriteria
                    {
                        SetId = ModelIdentifier.GetValue<TModel, long>(model)
                    },
                    RecordAction = recordAction
                };
                records.Add(record);
            }

            return true;
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(idProperty);
        }
    }
}