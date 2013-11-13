using System;
using System.Collections.Generic;
using AmplaData.Data.AmplaData2008;
using AmplaData.Data.Views;

namespace AmplaData.Data.Downtime
{
    public class DowntimeViews : StandardViews
    {
        public static GetView StandardView()
        {
            GetView view = new GetView
                {
                    name = "Downtime.StandardView",
                    DisplayName = "Downtime",
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
                    Field<string>("ConfirmedBy"),
                    Field<DateTime>("ConfirmedDateTime"),
                    Field<int>("MaskedById", "MaskedById", true),
                    Field<double>("ClippedPercentage", "ClippedPercentage", true),
                    Field<bool>("IsDeleted", "Deleted"),
                    Field<bool>("IsConfirmed", "Confirmed", true),
                    Field<bool>("IsMasked", "Masked", true),
                    Field<bool>("IsSplit", "IsSplit", true),
                    Field<bool>("IsVirtual", "Virtual", true),
                    Field<bool>("IsClipped", "Is Clipped", true),
                    Field<DateTime>("LastModified"),
                    Field<DateTime>("StartDateTime", "Start Time"),
                    Field<DateTime>("StartDateTime.Clipped", "Start Time (Clipped)", true),
                    Field<DateTime>("EndDateTime", "End Time"),
                    Field<DateTime>("EndDateTime.Clipped", "End Time (Clipped)", true),
                    Field<int>("Duration", "Duration", true),
                    Field<double>("Duration.Clipped", "Duration (Clipped)", true),
                    Field<string>("ObjectId", "Location"),
                    Field<string>("EquipmentId", "Equipment Id (Location)", true),
                    Field<string>("EquipmentType", "Equipment Type (Location)", true),
                    Field<string>("CauseLocationEquipmentId", "Equipment Id (Cause Location)", true),
                    Field<string>("CauseLocationEquipmentType", "Equipment Type (Cause Location)", true),
                    Field<double>("EffectiveDuration", "Eff. Duration", true),
                    Field<double>("EffectiveDuration.Clipped", "Eff. Duration (Clipped)", true),
                    Field<string>("Effect"),
                    Field<string>("Event"),
                    Field<string>("Cause Location", "Cause Location", false, false, true),
                    Field<string>("Cause"),
                    Field<string>("Classification"),
                    Field<string>("Explanation"),
                    Field<string>("Comments"),
                    Field<double>("PercentDowntime", "Eff. %"),
                };
            fields.AddRange(extraFields);
            return fields.ToArray();
        }
    }
}