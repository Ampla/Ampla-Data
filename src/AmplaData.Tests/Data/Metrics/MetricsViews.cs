using System;
using System.Collections.Generic;
using AmplaData.Data.AmplaData2008;
using AmplaData.Data.Views;

namespace AmplaData.Data.Metrics
{
    public class MetricsViews : StandardViews
    {

        public static GetView StandardView()
        {
            GetView view = new GetView
                {
                    name = "Metrics.StandardView",
                    DisplayName = "Metrics",
                    Fields = StandardFieldsPlus(),
                    AllowedOperations = AllowAll().Disallow(ViewAllowedOperations.SplitRecord),
                };
            return view;
        }

        public static GetView TotalTonnesView()
        {
            GetView view = new GetView
            {
                name = "Metrics.StandardView",
                DisplayName = "Metrics",
                Fields = StandardFieldsPlus(Field<double>("Total Tonnes")),
                AllowedOperations = AllowAll().Disallow(ViewAllowedOperations.SplitRecord),
            };
            return view;
        }

        private static GetViewsField[] StandardFieldsPlus(params GetViewsField[] extraFields)
        {
            List<GetViewsField> fields = new List<GetViewsField>
                {
                    Field<int>("Id", "Id", true, true),
                    Field<bool>("IsManual"),
                    Field<bool>("HasAudit"),
                    Field<string>("CreatedBy"),
                    Field<DateTime>("CreatedDateTime"),
                    Field<bool>("IsDeleted", "Deleted"),
                    Field<bool>("IsCalculated", "Calculated", true),
                    Field<DateTime>("LastModified"),
                    Field<DateTime>("StartDateTime", "Start Time", true),
                    Field<DateTime>("EndDateTime", "End Time", true),
                    Field<int>("Duration", "Duration", true),
                    Field<string>("Period", "Period", true),
                    Field<string>("ObjectId", "Location", true, true),
                };

            fields.AddRange(extraFields);
            return fields.ToArray();
        }
    }
}