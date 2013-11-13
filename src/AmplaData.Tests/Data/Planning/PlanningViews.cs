using System;
using System.Collections.Generic;
using AmplaData.Data.AmplaData2008;
using AmplaData.Data.Views;

namespace AmplaData.Data.Planning
{
    public class PlanningViews : StandardViews
    {

        public static GetView StandardView()
        {
            GetView view = new GetView
                {
                    name = "Planning.StandardView",
                    DisplayName = "Planning",
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
                    Field<string>("StateChangedBy"),
                    Field<DateTime>("StateChangedDateTime"),
                    //Field<string>("ConfirmedBy", "ConfirmedBy", true),
                    //Field<DateTime>("ConfirmedDateTime", "ConfirmedDateTime", true),
                    Field<bool>("IsDeleted", "Deleted"),
                    //Field<bool>("IsConfirmed", "Confirmed", true),
                    Field<DateTime>("LastModified"),
                    Field<DateTime>("PlannedStartDateTime", "Planned Start Time", false, true),
                    Field<DateTime>("PlannedEndDateTime", "Planned End Time", false, true),
                    Field<DateTime>("ActualStartDateTime", "Actual Start Time"),
                    Field<DateTime>("ActualEndDateTime", "Actual End Time"),
                    Field<string>("ObjectId", "Location", true, true),
                    Field<string>("ActivityId"),
                    Field<int>("State", "State", true, false),
                    Field<string>("Product", "Product", false, false, true),
                    Field<double>("RequiredQuantity", "Required Quantity"),
                    Field<string>("RequiredQuantityUnits", "Required Quantity Units"),
                    //Field<bool>("EquipmentId", "Equipment Id", true)
                };

            fields.AddRange(extraFields);
            return fields.ToArray();
        }
    }
}