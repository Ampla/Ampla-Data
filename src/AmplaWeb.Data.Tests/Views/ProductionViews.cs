using AmplaWeb.Data.AmplaData2008;

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

        public static GetView AreaValueModelView()
        {
            GetView view = new GetView();

            view.name = "Production.StandardView";
            view.DisplayName = "Standard View";
            view.Fields = new []
                {
                    LocationField(),
                    DateTimeField("Sample Period", "SampleDateTime"),
                    StringField("Area"),
                    DoubleField("Value")
                };

            return view;
        }

        private static GetViewsField DateTimeField(string displayName, string name)
        {
            GetViewsField field = new GetViewsField();
            field.name = name;
            field.type = "xs:DateTime";
            field.displayName = displayName;
            field.hasAllowedValues = false;
            field.hasRelationshipMatrixValues = false;
            field.readOnly = true;
            return field;
        }

        private static GetViewsField LocationField()
        {
            GetViewsField field = new GetViewsField();
            field.name = "ObjectId";
            field.type = "xs:String";
            field.displayName = "Location";
            field.hasAllowedValues = false;
            field.hasRelationshipMatrixValues = false;
            field.readOnly = true;
            return field;
        }

        private static GetViewsField StringField(string name)
        {
            GetViewsField field = new GetViewsField();
            field.name = name;
            field.type = "xs:String";
            field.displayName = name;
            field.hasAllowedValues = false;
            field.hasRelationshipMatrixValues = false;
            field.readOnly = false;
            return field;
        }

        private static GetViewsField DoubleField(string name)
        {
            GetViewsField field = new GetViewsField();
            field.name = name;
            field.type = "xs:Double";
            field.displayName = name;
            field.hasAllowedValues = false;
            field.hasRelationshipMatrixValues = false;
            field.readOnly = false;
            return field;
        }

    }
}