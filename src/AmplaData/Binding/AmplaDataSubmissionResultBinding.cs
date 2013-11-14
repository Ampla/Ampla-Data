using System;
using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.Binding.ModelData;

namespace AmplaData.Binding
{
    public class AmplaDataSubmissionResultBinding<TModel> : IAmplaBinding where TModel : new()
    {
        private readonly DataSubmissionResult[] dataSubmissionResults;
        private readonly List<TModel> models;
        private readonly IModelProperties<TModel> modelProperties;
        private readonly string idProperty;

        public AmplaDataSubmissionResultBinding(DataSubmissionResult[] dataSubmissionResults, List<TModel> models, IModelProperties<TModel> modelProperties )
        {
            this.dataSubmissionResults = dataSubmissionResults ?? new DataSubmissionResult[0];
            this.models = models;
            this.modelProperties = modelProperties;
            idProperty = ModelIdentifier.GetPropertyName<TModel>();
        }

        public bool Bind()
        {
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

        public bool Validate()
        {
            return !string.IsNullOrEmpty(idProperty);
        }
    }
}