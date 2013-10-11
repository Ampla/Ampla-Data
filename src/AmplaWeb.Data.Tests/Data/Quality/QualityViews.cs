using System;
using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Views;

namespace AmplaWeb.Data.Quality
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

        /*
         * 
              <Field name="IsManual" type="xs:Boolean" displayName="IsManual" localizedDisplayName="IsManual" />
              <Field name="HasAudit" type="xs:Boolean" displayName="HasAudit" localizedDisplayName="HasAudit" />
              <Field name="CreatedBy" type="xs:String" displayName="CreatedBy" localizedDisplayName="CreatedBy" />
              <Field name="CreatedDateTime" type="xs:DateTime" displayName="CreatedDateTime" localizedDisplayName="CreatedDateTime" />
              <Field name="ConfirmedBy" type="xs:String" displayName="ConfirmedBy" localizedDisplayName="ConfirmedBy" readOnly="true" />
              <Field name="ConfirmedDateTime" type="xs:DateTime" displayName="ConfirmedDateTime" localizedDisplayName="ConfirmedDateTime" readOnly="true" />
              <Field name="IsDeleted" type="xs:Boolean" displayName="Deleted" localizedDisplayName="Deleted" />
              <Field name="IsConfirmed" type="xs:Boolean" displayName="Confirmed" localizedDisplayName="Confirmed" readOnly="true" />
              <Field name="LastModified" type="xs:DateTime" displayName="LastModified" localizedDisplayName="LastModified" />
              <Field name="QualityType" type="xs:Byte" displayName="Quality Type" localizedDisplayName="Quality Type" readOnly="true" required="true" />
              <Field name="SampleDateTime" type="xs:DateTime" displayName="Sample Period" localizedDisplayName="Sample Period" required="true" />
              <Field name="Duration" type="xs:Int" displayName="Duration" localizedDisplayName="Duration" />
              <Field name="ObjectId" type="xs:String" displayName="Location" localizedDisplayName="Location" readOnly="true" required="true" />
              <Field name="EquipmentId" type="xs:String" displayName="Equipment Id" localizedDisplayName="Equipment Id" readOnly="true" />
         */

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