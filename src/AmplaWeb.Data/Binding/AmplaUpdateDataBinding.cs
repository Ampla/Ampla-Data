using System;
using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.Mapping;
using AmplaWeb.Data.Binding.ModelData;
using AmplaWeb.Data.Binding.ViewData;

namespace AmplaWeb.Data.Binding
{
    public class AmplaUpdateDataBinding<TModel> : AmplaBinding where TModel : new()
    {
        private readonly List<TModel> models;
        private readonly List<SubmitDataRecord> records;
        private readonly AmplaViewProperties<TModel> amplaViewProperties;

        public AmplaUpdateDataBinding(List<TModel> models, List<SubmitDataRecord> records, AmplaViewProperties<TModel> amplaViewProperties)
        {
            this.models = models;
            this.records = records;
            this.amplaViewProperties = amplaViewProperties;
        }

        public override bool Bind()
        {
            if (models.Count == 0) return false;

            ModelProperties<TModel> modelProperties = new ModelProperties<TModel>();

            foreach (TModel model in models)
            {
                SubmitDataRecord record = new SubmitDataRecord();
                foreach (FieldMapping fieldMapping in amplaViewProperties.GetFieldMappings())
                {
                    throw new NotImplementedException();
                }
                records.Add(record);
            }

            return true;
        }

    }
}