using System.Collections.Generic;
using AmplaData.Data.AmplaData2008;
using AmplaData.Data.Binding.ModelData;

namespace AmplaData.Data.Binding
{
    public class AmplaDeleteDataBinding<TModel> : IAmplaBinding where TModel : new()
    {
        private readonly List<TModel> models;
        private readonly List<DeleteRecord> records;
        private readonly IModelProperties<TModel> modelProperties;
        private readonly string idProperty;

        public AmplaDeleteDataBinding(List<TModel> models, List<DeleteRecord> records, IModelProperties<TModel> modelProperties )
        {
            this.models = models;
            this.records = records;
            this.modelProperties = modelProperties;
            idProperty = ModelIdentifier.GetPropertyName<TModel>();
        }

        public bool Bind()
        {
            if (models.Count == 0) return false;

            foreach (TModel model in models)
            {
                DeleteRecord record = new DeleteRecord
                    {
                        Location = modelProperties.GetLocation(model),
                        Module = modelProperties.Module,
                        MergeCriteria = new DeleteRecordsMergeCriteria
                            {
                                SetId = ModelIdentifier.GetValue<TModel, long>(model)
                            }
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