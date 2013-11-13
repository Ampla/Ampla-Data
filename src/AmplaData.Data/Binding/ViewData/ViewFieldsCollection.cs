using AmplaData.Data.AmplaData2008;

namespace AmplaData.Data.Binding.ViewData
{
    public class ViewFieldsCollection : ViewCollection<ViewField>
    {
        public void Initialise(GetView view)
        {
            foreach (GetViewsField field in view.Fields)
            {
                ViewField viewField = new ViewField(field);
                Add(viewField.Name, viewField);
            }
        }

        public ViewField FindByName(string name)
        {
            return Find(vf => vf.Name == name);
        }

        public ViewField FindByDisplayName(string displayName)
        {
            return Find(vf => vf.DisplayName == displayName);
        }
    }
}