using System;
using System.Collections.Generic;
using AmplaData.Data.AmplaData2008;
using AmplaData.Data.Views;

namespace AmplaData.Data.Knowledge
{
    public class KnowledgeViews : StandardViews
    {
        public static GetView StandardView()
        {
            GetView view = new GetView
                {
                    name = "Knowledge.DiaryView",
                    DisplayName = "Knowledge",
                    Fields = StandardFieldsPlus(),
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
                    Field<string>("ConfirmedBy"),
                    Field<DateTime>("ConfirmedDateTime"),
                    Field<bool>("IsDeleted", "Deleted"),
                    Field<bool>("IsConfirmed", "Confirmed", true),
                    Field<DateTime>("LastModified"),
                    Field<DateTime>("SampleDateTime", "Sample Period"),
                    Field<int>("Duration"),
                    Field<string>("ObjectId", "Location"),
                    Field<bool>("EquipmentId", "Equipment Id", true),
                    Field<string>("Priority"),
                    Field<string>("Topic"),
                    Field<string>("Memo"),
                    Field<string>("Sketch"),
                };
            fields.AddRange(extraFields);
            return fields.ToArray();
        }
    }
}