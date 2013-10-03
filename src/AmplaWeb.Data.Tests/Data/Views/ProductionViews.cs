using System;
using System.Collections.Generic;
using System.Linq;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.MetaData;

namespace AmplaWeb.Data.Views
{
    public static class ProductionViewExtensions
    {
        public static GetViewsAllowedOperation[] AllowAll(this GetViewsAllowedOperation[] existingOperations)
        {
            return existingOperations.Allow(AllOperations);
        }

        public static GetViewsAllowedOperation[] Allow(this GetViewsAllowedOperation[] existingOperations,
                                                       params ViewAllowedOperations[] operations)
        {
            List<ViewAllowedOperations> allowedOperations = new List<ViewAllowedOperations>(operations ?? new ViewAllowedOperations[0]);
            List<ViewAllowedOperations> existingPermissions = (from operation in existingOperations where operation.Allowed select operation.Operation).ToList();

            return AllOperations.Select(operation => new GetViewsAllowedOperation
                {
                    Operation = operation, Allowed = existingPermissions.Contains(operation) || allowedOperations.Contains(operation)
                }).ToArray();
        }

        public static readonly ViewAllowedOperations[] AllOperations = new[]
            {
                ViewAllowedOperations.AddRecord, 
                ViewAllowedOperations.ConfirmRecord,
                ViewAllowedOperations.DeleteRecord, 
                ViewAllowedOperations.ModifyRecord,
                ViewAllowedOperations.SplitRecord, 
                ViewAllowedOperations.UnconfirmRecord,
                ViewAllowedOperations.ViewRecord
            };

    }

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

        public static GetViewsAllowedOperation[] AllowAll()
        {
            return new GetViewsAllowedOperation[0].AllowAll();
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
            GetViewsField field = new GetViewsField
                {
                    name = name,
                    type = DataTypeHelper.GetAmplaDataType<T>(),
                    displayName = displayName,
                    hasAllowedValues = false,
                    hasRelationshipMatrixValues = false,
                    readOnly = isReadOnly,
                    required = required
                };
            return field;
        }

    }
}