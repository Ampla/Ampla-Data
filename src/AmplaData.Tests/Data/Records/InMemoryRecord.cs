using System;
using System.Collections.Generic;
using System.Linq;
using AmplaData.AmplaData2008;

namespace AmplaData.Records
{
    public class InMemoryRecord
    {
        public InMemoryRecord()
        {
            Fields = new List<FieldValue>();
            RecordId = 0;
        }

        public string Location
        {
            get { return GetFieldValue("Location", string.Empty); }
            set { SetFieldValue("Location", value); }
        }

        public int RecordId
        {
            get; set;
        }

        public string Module
        {
            get;
            set;
        }

        public IList<FieldValue> Fields { get; private set; }


        public InMemoryRecord Clone()
        {
            InMemoryRecord record = new InMemoryRecord { Location = Location, Module = Module, RecordId = RecordId };

            foreach (FieldValue value in Fields)
            {
                record.Fields.Add(value.Clone());
            }
            return record;
        }

        public FieldValue Find(string fieldName)
        {
            return Fields.FirstOrDefault(field => StringComparer.InvariantCulture.Compare(field.Name, fieldName) == 0);
        }

        public T GetFieldValue<T>(string fieldName, T defaultValue)
        {
            FieldValue field = Find(fieldName);
            return field == null ? defaultValue : field.GetValue<T>();
        }

        public SubmitDataRecord ConvertToSubmitDataRecord()
        {
            MergeCriteria mergeCriteria = RecordId > 0
                      ? new MergeCriteria { SetId = RecordId }
                      : null;

            AmplaModules amplaModule;
            Enum.TryParse(Module, out amplaModule);
            SubmitDataRecord submitRecord = new SubmitDataRecord
            {
                Location = Location,
                Module = amplaModule,
                MergeCriteria = mergeCriteria,
                Fields = FieldsToUpdate().Select(fieldValue => new Field { Name = fieldValue.Name, Value = fieldValue.Value }).ToArray()
            };

            return submitRecord;
        }

        public IEnumerable<FieldValue> FieldsToUpdate()
        {
            return Fields.Where(fieldValue => (StringComparer.InvariantCulture.Compare(fieldValue.Name, "Location") != 0));
        }

        public void SetFieldValue<T>(string fieldName, T value)
        {
            FieldValue field = Find(fieldName);
            if (field == null)
            {
                Fields.Add(new FieldValue(fieldName, PersistenceHelper.ConvertToString(value)));
            }
            else
            {
                field.SetValue(value);
            }
        }

        public void SetFieldIdValue<T>(string fieldName, T value, int id)
        {
            FieldValue field = Find(fieldName);
            if (field == null)
            {
                Fields.Add(new FieldValue(fieldName, PersistenceHelper.ConvertToString(value), id));
            }
            else
            {
                field.SetIdValue(value, id);
            }
        }
    }

  
}