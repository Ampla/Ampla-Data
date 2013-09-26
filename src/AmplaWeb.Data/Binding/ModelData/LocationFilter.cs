namespace AmplaWeb.Data.Binding.ModelData
{
    /// <summary>
    ///     Location Filter 
    /// </summary>
    public class LocationFilter
    {
        public LocationFilter(string location, bool withRecurse)
        {
            Location = location;
            WithRecurse = withRecurse;
        }
        public string Filter
        {
            get
            {
                return Location + (WithRecurse ? " with recurse" : "");
            }
        }

        public string Location
        {
            get; private set;
        }

        public bool WithRecurse
        {
            get; private set;
        }
    }
}