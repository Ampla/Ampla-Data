﻿using System.Collections.Generic;
using System.Xml;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding
{
    public class AmplaGetDataBinding<TModel> : AmplaBinding where TModel : new()
    {
        private readonly GetDataResponse response;
        private readonly List<TModel> records;

        public AmplaGetDataBinding(GetDataResponse response, List<TModel> records)
        {
            this.response = response;
            this.records = records;
        }

        public override bool Bind()
        {
            if (response.RowSets.Length == 0) return false;

            ModelProperties<TModel> modelProperties = new ModelProperties<TModel>();

            RowSet rowSet = response.RowSets[0];
            
            string idPropertyName = ModelIdentifier.GetPropertyName<TModel>();

            foreach (Row row in rowSet.Rows)
            {
                TModel model = new TModel();

                modelProperties.TrySetValueFromString(model, idPropertyName, row.id);

                foreach (XmlElement cell in row.Any)
                {
                    string field = XmlConvert.DecodeName(cell.Name);

                    modelProperties.TrySetValueFromString(model, field, cell.InnerText);
                }
                records.Add(model);
            }
            return true;
        }

    }
}