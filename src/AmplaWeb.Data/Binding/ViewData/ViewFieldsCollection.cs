using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Binding.ViewData
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

        public ViewField Find(string name)
        {
            return Find((vf) => vf.Name == name);
        }
    }
}