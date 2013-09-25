using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding
{
    public class AmplaDeleteDataBinding<TModel> : IAmplaBinding where TModel : new()
    {
        private readonly List<TModel> models;
        private readonly List<DeleteRecord> records;
        private readonly IModelProperties<TModel> modelProperties;

        public AmplaDeleteDataBinding(List<TModel> models, List<DeleteRecord> records, IModelProperties<TModel> modelProperties )
        {
            this.models = models;
            this.records = records;
            this.modelProperties = modelProperties;
        }

        public bool Bind()
        {
            if (models.Count == 0) return false;

            string propertyId = ModelIdentifier.GetPropertyName<TModel>();

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

    }
}