using System.Collections.Generic;
using System.Xml;
using AmplaData.AmplaData2008;
using AmplaData.Binding;
using AmplaData.Binding.MetaData;
using AmplaData.Dynamic.Binding.ModelData;

namespace AmplaData.Dynamic.Binding
{
    /// <summary>
    ///     Ampla Data Binding from GetData Response to Dynamic object
    /// </summary>
    public class AmplaGetDataDynamicBinding : IAmplaBinding
    {
        private readonly GetDataResponse response;
        private readonly List<object> records;
        private readonly IDynamicModelProperties modelProperties;

        public AmplaGetDataDynamicBinding(GetDataResponse response, List<dynamic> records, IDynamicModelProperties modelProperties)
        {
            this.response = response;
            this.records = records;
            this.modelProperties = modelProperties;
        }

        public bool Bind()
        {
            if (response.RowSets.Length == 0) return false;

            string module = response.Context.Module.ToString();

            RowSet rowSet = response.RowSets[0];

            foreach (Row row in rowSet.Rows)
            {
                IRecordLoad record = new DynamicRecord(module);

                foreach (var column in rowSet.Columns)
                {
                    record.AddColumn(column.displayName, DataTypeHelper.GetDataType(column.type));
                }

                modelProperties.TrySetValueFromString(record, "Id", row.id);

                record.SetValue("Id", row.id);
                foreach (XmlElement cell in row.Any)
                {
                    string field = XmlConvert.DecodeName(cell.Name);
                    string value = cell.InnerText;
                    modelProperties.TrySetValueFromString(record, field, value);

                    record.SetValue(field, value);
                }

                records.Add(record);
            }
            return true;
        }

        public bool Validate()
        {
            return true;
        }
    }
}