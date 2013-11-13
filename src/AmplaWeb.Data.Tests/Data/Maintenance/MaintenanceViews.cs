using System;
using System.Collections.Generic;
using AmplaData.Data.AmplaData2008;
using AmplaData.Data.Views;

namespace AmplaData.Data.Maintenance
{
    public class MaintenanceViews : StandardViews
    {
        public static GetView StandardView()
        {
            GetView view = new GetView
                {
                    name = "Maintenance.StandardView",
                    DisplayName = "Maintenance",
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
                    Field<string>("ConfirmedBy", "ConfirmedBy"),
                    Field<DateTime>("ConfirmedDateTime", "ConfirmedDateTime"),
                    Field<bool>("IsDeleted", "Deleted"),
                    Field<bool>("IsConfirmed", "Confirmed", true),
                    Field<DateTime>("LastModified"),
                    Field<DateTime>("SampleDateTime", "Sample Period"),
                    Field<int>("Duration"),
                    Field<string>("ObjectId", "Location"),
                    Field<bool>("EquipmentId", "Equipment Id", true)
                };

            fields.AddRange(extraFields);
            return fields.ToArray();
        }
    }
}