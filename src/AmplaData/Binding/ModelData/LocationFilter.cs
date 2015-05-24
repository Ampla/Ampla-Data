using System;

namespace AmplaData.Binding.ModelData
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

        public static bool TryParse(string locationFilter, out LocationFilter filter)
        {
            if (!string.IsNullOrEmpty(locationFilter))
            {
                string location = locationFilter;
                bool withRecurse = false;
                if (locationFilter.EndsWith(" with recurse", StringComparison.InvariantCultureIgnoreCase))
                {
                    int withRecurseLenth = " with recurse".Length;
                    if (locationFilter.Length > withRecurseLenth)
                    {
                        location = locationFilter.Substring(0, locationFilter.Length - withRecurseLenth);
                        withRecurse = true;
                    }
                }
                filter = new LocationFilter(location, withRecurse);
                return true;
            }
            
            filter = null;
            return false;
        }
    }
}