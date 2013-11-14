using AmplaData.AmplaData2008;

namespace AmplaData.Binding.ViewData
{
    public class ViewPeriod
    {
        public ViewPeriod(GetViewsPeriod period)
        {
            Name = period.name;
            DisplayName = period.displayName;

        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}