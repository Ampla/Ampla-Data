using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.Mapping;
using AmplaWeb.Data.Binding.ModelData;
using AmplaWeb.Data.Binding.ModelData.Validation;
using AmplaWeb.Data.Binding.ViewData;

namespace AmplaWeb.Data.Binding
{
    public class AmplaUpdateDataBinding<TModel> : IAmplaBinding where TModel : class, new()
    {
        private readonly TModel existing;
        private readonly TModel update;
        private readonly List<SubmitDataRecord> records;
        private readonly IAmplaViewProperties amplaViewProperties;
        private readonly IModelProperties<TModel> modelProperties;

        public AmplaUpdateDataBinding(TModel existing, TModel update, List<SubmitDataRecord> records, IAmplaViewProperties amplaViewProperties, IModelProperties<TModel> modelProperties)
        {
            this.existing = existing;
            this.update = update;
            this.records = records;
            this.amplaViewProperties = amplaViewProperties;
            this.modelProperties = modelProperties;
        }

        public bool Bind()
        {
            if (existing == null) return false;
            if (update == null) return false;

            if (modelProperties.GetLocation(existing) != modelProperties.GetLocation(update)) return false;

            SubmitDataRecord record = new SubmitDataRecord
            {
                Location = modelProperties.GetLocation(existing),
                Module = modelProperties.Module,
                MergeCriteria = new MergeCriteria
                {
                    SetId = ModelIdentifier.GetValue<TModel, long>(update)
                }
            };

            List<Field> fields = new List<Field>();
            foreach (FieldMapping fieldMapping in amplaViewProperties.GetFieldMappings())
            {
                if (fieldMapping.CanWrite)
                {
                    string existingField;
                    string updateField;
                    if (fieldMapping.TryResolveValue(modelProperties, existing, out existingField)
                        && fieldMapping.TryResolveValue(modelProperties, update, out updateField))
                    {
                        if (updateField != existingField)
                        {
                            Field field = new Field {Name = fieldMapping.Name, Value = updateField};
                            fields.Add(field);
                        }
                    }
                }
            }
            if (fields.Count > 0)
            {
                record.Fields = fields.ToArray();
                records.Add(record);
            }
            
            return records.Count > 0;
        }

        public bool Validate()
        {
            ValidationMessages validationMessages = new ValidationMessages();

            string existingLocation = modelProperties.GetLocation(existing);

            RequiredLocationValidator<TModel> requiredLocation = new RequiredLocationValidator<TModel>(existingLocation);
            
            bool isValid = requiredLocation.Validate(modelProperties, update, validationMessages);
            isValid &= modelProperties.ValidateModel(update, validationMessages);
            
            if (!isValid)
            {
                validationMessages.Throw();
            }

            return isValid;
        }
    }
}