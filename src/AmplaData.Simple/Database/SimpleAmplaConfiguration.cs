using System;
using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.Views;

namespace AmplaData.Database
{
    public class SimpleAmplaConfiguration : IAmplaConfiguration
    {
        private readonly Dictionary<string, List<string>> locationsByModule = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, GetView> defaultViewByModule = new Dictionary<string, GetView>();
        
        public void EnableModule(string module)
        {
            locationsByModule.Add(module, new List<string>());
            defaultViewByModule.Add(module, StandardViews.EmptyView());
        }

        public void AddLocation(string module, string location)
        {
            List<string> locations;
            if (locationsByModule.TryGetValue(module, out locations))
            {
                locations.Add(location);
            }
            else
            {
                throw new ArgumentException("Invalid Module: " + module);
            }
        }

        public string[] GetLocations(string module)
        {
            List<string> locations;
            if (locationsByModule.TryGetValue(module, out locations))
            {
                return locations.ToArray();
            }

            return new string[0];
        }

        public GetView GetViewForLocation(string module, string location)
        {
            GetView view;
            if (defaultViewByModule.TryGetValue(module, out view))
            {
                return view;
            }
            throw new ArgumentException("Invalid Module: " + module);
        }

        public void SetDefaultView(string module, GetView defaultView)
        {
            GetView view;
            if (defaultViewByModule.TryGetValue(module, out view))
            {
                defaultViewByModule[module] = defaultView;
            }
            else
            {
                throw new ArgumentException("Invalid Module: " + module);
            }
        }
    }
}