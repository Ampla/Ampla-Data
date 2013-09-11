using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.Mapping;
using AmplaWeb.Data.Binding.ModelData;
using AmplaWeb.Data.Binding.ViewData;

namespace AmplaWeb.Data.Binding
{
    public class AmplaAddDataBinding<TModel> : AmplaBinding where TModel : new()
    {
        private readonly List<TModel> models;
        private readonly List<SubmitDataRecord> records;
        private readonly AmplaViewProperties<TModel> amplaViewProperties;
        private readonly ModelProperties<TModel> modelProperties;

        public AmplaAddDataBinding(List<TModel> models, List<SubmitDataRecord> records, AmplaViewProperties<TModel> amplaViewProperties, ModelProperties<TModel> modelProperties )
        {
            this.models = models;
            this.records = records;
            this.amplaViewProperties = amplaViewProperties;
            this.modelProperties = modelProperties;
        }

        public override bool Bind()
        {
            if (models.Count == 0) return false;

            foreach (TModel model in models)
            {
                SubmitDataRecord record = new SubmitDataRecord
                    {
                        Location = modelProperties.Location,
                        Module = modelProperties.Module,
                        MergeCriteria = null
                    };

                List<Field> fields = new List<Field>();
                foreach (FieldMapping fieldMapping in amplaViewProperties.GetFieldMappings())
                {
                    if (fieldMapping.CanWrite)
                    {
                        string value;
                        if (fieldMapping.TryResolveValue(modelProperties, model, out value))
                        {
                            Field field = new Field() {Name = fieldMapping.Name, Value = value};
                            fields.Add(field);
                        }
                    }
                }
                if (fields.Count > 0)
                {
                    record.Fields = fields.ToArray();
                    records.Add(record);
                }
            }

            return records.Count > 0;
        }

    }
}