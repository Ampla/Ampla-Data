using System;
using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Views
{
    public class ProductionViews : StandardViews
    {

        public static GetView StandardView()
        {
            GetView view = new GetView
                {
                    name = "Production.StandardView",
                    DisplayName = "Production",
                    Fields = StandardFieldsPlus(),
                    AllowedOperations = AllowAll(),
                };
            return view;
        }

        public static GetView AreaValueModelView()
        {
            GetView view = new GetView
                {
                    name = "Production.StandardView",
                    DisplayName = "Production",
                    Fields = StandardFieldsPlus(new[]
                        {
                            Field<string>("Area"),
                            Field<double>("Value")
                        }),
                    AllowedOperations = AllowAll()

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
                    Field<string>("ConfirmedBy"),
                    Field<DateTime>("ConfirmedDateTime"),
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