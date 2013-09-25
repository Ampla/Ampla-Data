using System;
using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.MetaData;

namespace AmplaWeb.Data.Views
{
    public class ProductionViews
    {
        public static GetView EmptyView()
        {
            return new GetView
            {
                Fields = new GetViewsField[0],
                AllowedOperations = new GetViewsAllowedOperation[0],
                Filters = new GetViewsFilter[0],
                Periods = new GetViewsPeriod[0]
            };
        }

        public static GetView StandardView()
        {
            GetView view = new GetView
                {
                    name = "Production.StandardView",
                    DisplayName = "Production",
                    Fields = StandardFieldsPlus()
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
                        })
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

        private static GetViewsField Field<T>(string name)
        {
            return Field<T>(name, name, false, false);
        }

        private static GetViewsField Field<T>(string name, string displayName)
        {
            return Field<T>(name, displayName, false, false);
        }

        private static GetViewsField Field<T>(string name, string displayName, bool isReadOnly)
        {
            return Field<T>(name, displayName, isReadOnly, false);
        }

        private static GetViewsField Field<T>(string name, string displayName, bool isReadOnly, bool required)
        {
            GetViewsField field = new GetViewsField();
            field.name = name;
            field.type = DataTypeHelper.GetAmplaDataType<T>();
            field.displayName = displayName;
            field.hasAllowedValues = false;
            field.hasRelationshipMatrixValues = false;
            field.readOnly = isReadOnly;
            field.required = required;
            return field;
        }

    }
}