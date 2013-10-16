using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml;
using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Binding
{
    public class AmplaGetDataDynamicBinding : IAmplaBinding 
    {
        //private readonly IModelProperties modelProperties;
        private readonly GetDataResponse response;
        private readonly List<dynamic> records;

        public AmplaGetDataDynamicBinding(GetDataResponse response, List<dynamic> records)
        {
            this.response = response;
            this.records = records;
        }

        public bool Bind()
        {
            if (response.RowSets.Length == 0) return false;

            RowSet rowSet = response.RowSets[0];

            List<string> columns = rowSet.Columns.Select(column => column.displayName).ToList();


            foreach (Row row in rowSet.Rows)
            {
                dynamic model = new ExpandoObject();
                IDictionary<string, object> dictionary = model as IDictionary<string, object>;
                foreach (string column in columns)
                {
                    dictionary[column] = string.Empty;
                }

                dictionary["id"] = row.id;
                //modelProperties.TrySetValueFromString(model, idPropertyName, row.id);

                foreach (XmlElement cell in row.Any)
                {
                    string field = XmlConvert.DecodeName(cell.Name);
                    string value = cell.InnerText;
                    dictionary[field] = value;
                    //modelProperties.TrySetValueFromString(model, field, cell.InnerText);
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