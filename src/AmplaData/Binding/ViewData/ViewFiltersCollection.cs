using AmplaData.AmplaData2008;

namespace AmplaData.Binding.ViewData
{
    public class ViewFiltersCollection : ViewCollection<ViewFilter>
    {
        public void Initialise(GetView view)
        {
            if (view != null && view.Filters != null)
            {
                foreach (GetViewsFilter filter in view.Filters)
                {
                    ViewFilter viewFilter = new ViewFilter(filter);
                    Add(viewFilter.Name, viewFilter);
                }
            }
        }
    }
}