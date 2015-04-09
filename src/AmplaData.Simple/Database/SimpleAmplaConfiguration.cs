using System;
using System.Collections.Generic;

namespace AmplaData.Database
{
    public class SimpleAmplaConfiguration : IAmplaConfiguration
    {
        private readonly Dictionary<string, List<string>> locationsByModule = new Dictionary<string, List<string>>();
        
        public void EnableModule(string module)
        {
            locationsByModule.Add(module, new List<string>());
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

            throw new ArgumentException("Invalid Module: " + module);
        }
    }
}