using System;
using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.Views;

namespace AmplaData.Modules.Quality
{
    public class QualityViews : StandardViews
    {

        public static GetView StandardView()
        {
            GetView view = new GetView
                {
                    name = "Quality.StandardView",
                    DisplayName = "Quality",
                    Fields = StandardFieldsPlus(),
                    AllowedOperations = AllowAll(),
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
                    Field<string>("ConfirmedBy", "ConfirmedBy", true),
                    Field<DateTime>("ConfirmedDateTime", "ConfirmedDateTime", true),
                    Field<bool>("IsDeleted", "Deleted"),
                    Field<bool>("IsConfirmed", "Confirmed", true),
                    Field<DateTime>("LastModified"),
                    Field<Byte>("QualityType", "Quality Type", true, true),
                    Field<DateTime>("SampleDateTime", "Sample Period", false, true),
                    Field<int>("Duration"),
                    Field<string>("ObjectId", "Location", true, true),
                    Field<bool>("EquipmentId", "Equipment Id", true)
                };

            fields.AddRange(extraFields);
            return fields.ToArray();
        }
    }
}