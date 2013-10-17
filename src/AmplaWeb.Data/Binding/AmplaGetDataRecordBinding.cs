using System;
using System.Collections.Generic;
using System.Xml;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.AmplaRepository;
using AmplaWeb.Data.Binding.MetaData;
using AmplaWeb.Data.Binding.ModelData;
using AmplaWeb.Data.Records;

namespace AmplaWeb.Data.Binding
{
    public class AmplaGetDataRecordBinding<TModel> : IAmplaBinding where TModel : new()
    {
        private readonly GetDataResponse response;
        private readonly List<AmplaRecord> records;
        private readonly IModelProperties<TModel> modelProperties;

        public AmplaGetDataRecordBinding(GetDataResponse response, List<AmplaRecord> records, IModelProperties<TModel> modelProperties)
        {
            this.response = response;
            this.records = records;
            this.modelProperties = modelProperties;
        }

        public bool Bind()
        {
            if (response.RowSets.Length == 0) return false;

            RowSet rowSet = response.RowSets[0];

            foreach (Row row in rowSet.Rows)
            {
                AmplaRecord model = new AmplaRecord(Convert.ToInt32(row.id))
                    {
                        Module = modelProperties.Module.ToString()
                    };

                foreach (var column in rowSet.Columns)
                {
                    model.AddColumn(column.displayName, DataTypeHelper.GetDataType(column.type));
                }
                
                foreach (XmlElement cell in row.Any)
                {
                    string field = XmlConvert.DecodeName(cell.Name);
                    string value = cell.InnerText;
                    model.SetValue(field, value);
                }
                records.Add(model);
            }
            return true;
        }

        public bool Validate()
        {
            return true;
        }
    }
}