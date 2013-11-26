using System;
using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.Binding;

namespace AmplaData.Dynamic.Binding
{
    public class AmplaDataSubmissionResultDynamicBinding : IAmplaBinding 
    {
        private readonly DataSubmissionResult[] dataSubmissionResults;
        private readonly List<object> models;

        public AmplaDataSubmissionResultDynamicBinding(DataSubmissionResult[] dataSubmissionResults, List<object> models)
        {
            this.dataSubmissionResults = dataSubmissionResults ?? new DataSubmissionResult[0];
            this.models = models;
        }

        public bool Bind()
        {
            if (models.Count == dataSubmissionResults.Length)
            {
                for (int i = 0; i < dataSubmissionResults.Length; i++)
                {
                    dynamic model = models[i];
                    DataSubmissionResult result = dataSubmissionResults[i];

                    if (result.RecordAction == RecordAction.Insert)
                    {
                        model.Id = Convert.ToInt32(result.SetId);
                    }
                }
            }
            return true;
        }

        public bool Validate()
        {
            return true;
        }
    }
}