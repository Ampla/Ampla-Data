using AmplaData.AmplaData2008;
using AmplaData.Binding.MetaData;

namespace AmplaData.Views
{
    public class StandardViews
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

        protected static GetViewsAllowedOperation[] AllowAll()
        {
            return new GetViewsAllowedOperation[0].AllowAll();
        }

        public static GetViewsField Field<T>(string name)
        {
            return Field<T>(name, name, false, false);
        }

        public static GetViewsField Field<T>(string name, string displayName)
        {
            return Field<T>(name, displayName, false, false);
        }

        public static GetViewsField Field<T>(string name, string displayName, bool isReadOnly)
        {
            return Field<T>(name, displayName, isReadOnly, false);
        }

        public static GetViewsField Field<T>(string name, string displayName, bool isReadOnly, bool required)
        {
            return Field<T>(name, displayName, isReadOnly, required, false);
        }

        public static GetViewsField Field<T>(string name, string displayName, bool isReadOnly, bool required, bool hasValues)
        {
            GetViewsField field = new GetViewsField
            {
                name = name,
                type = DataTypeHelper.GetAmplaDataType<T>(),
                displayName = displayName,
                hasAllowedValues = hasValues,
                hasRelationshipMatrixValues = false,
                readOnly = isReadOnly,
                required = required
            };
            return field;
        }
    }
}