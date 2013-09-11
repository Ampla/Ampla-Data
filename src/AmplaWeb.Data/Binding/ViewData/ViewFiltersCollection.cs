using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Binding.ViewData
{
    public class ViewFiltersCollection : ViewCollection<ViewFilter>
    {
        public void Initialise(GetView view)
        {
            foreach (GetViewsFilter filter in view.Filters)
            {
                ViewFilter viewFilter = new ViewFilter(filter);
                Add(viewFilter.Name, viewFilter);
            }
        }
    }
}