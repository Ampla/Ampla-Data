using System;
using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding
{
    public class AmplaDataSubmissionResultBinding<TModel> : IAmplaBinding where TModel : new()
    {
        private readonly DataSubmissionResult[] dataSubmissionResults;
        private readonly List<TModel> models;
        private readonly IModelProperties<TModel> modelProperties;

        public AmplaDataSubmissionResultBinding(DataSubmissionResult[] dataSubmissionResults, List<TModel> models, IModelProperties<TModel> modelProperties )
        {
            this.dataSubmissionResults = dataSubmissionResults ?? new DataSubmissionResult[0];
            this.models = models;
            this.modelProperties = modelProperties;
        }

        public bool Bind()
        {
            string idProperty = ModelIdentifier.GetPropertyName<TModel>();
            if (string.IsNullOrEmpty(idProperty))
            {
                return false;
            }

            if (models.Count == dataSubmissionResults.Length)
            {
                for (int i = 0; i < dataSubmissionResults.Length; i++)
                {
                    TModel model = models[i];
                    DataSubmissionResult result = dataSubmissionResults[i];

                    if (result.RecordAction == RecordAction.Insert)
                    {
                        modelProperties.TrySetValueFromString(model, idProperty, Convert.ToString(result.SetId));
                    }
                }
            }
            return true;
        }
    }
}