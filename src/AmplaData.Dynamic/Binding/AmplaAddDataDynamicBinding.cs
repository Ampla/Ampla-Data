using System.Collections.Generic;
using System.Linq;
using AmplaData.AmplaData2008;
using AmplaData.Binding;
using AmplaData.Binding.Mapping;
using AmplaData.Binding.ModelData.Validation;
using AmplaData.Dynamic.Binding.ModelData;
using AmplaData.Dynamic.Binding.ViewData;

namespace AmplaData.Dynamic.Binding
{
    public class AmplaAddDataDynamicBinding : IAmplaBinding
    {
        private readonly List<object> models;
        private readonly List<SubmitDataRecord> records;
        private readonly IDynamicAmplaViewProperties amplaViewProperties;
        private readonly IDynamicModelProperties modelProperties;

        public AmplaAddDataDynamicBinding(List<dynamic> models, List<SubmitDataRecord> records,
                                          IDynamicAmplaViewProperties amplaViewProperties,
                                          IDynamicModelProperties modelProperties)
        {
            this.models = models;
            this.records = records;
            this.amplaViewProperties = amplaViewProperties;
            this.modelProperties = modelProperties;
        }

        public bool Bind()
        {
            if (models.Count == 0) return false;

            foreach (object model in models)
            {
                SubmitDataRecord record = new SubmitDataRecord
                    {
                        Location = modelProperties.GetLocation(model),
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
                            Field field = new Field {Name = fieldMapping.Name, Value = value};
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

        public bool Validate()
        {
            ValidationMessages validationMessages = new ValidationMessages();
            bool isValid = models.Aggregate(true, (current, model) => current & modelProperties.ValidateModel(model, validationMessages));

            if (!isValid)
            {
                validationMessages.Throw();
            }

            return isValid;
        }
    }
}