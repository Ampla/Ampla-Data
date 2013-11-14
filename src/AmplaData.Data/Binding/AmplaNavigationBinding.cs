using System.Collections.Generic;
using AmplaData.AmplaData2008;


namespace AmplaData.Binding
{
    public class AmplaNavigationBinding : IAmplaBinding
    {
        private readonly GetNavigationHierarchyResponse response;
        private readonly List<string> values;

        public AmplaNavigationBinding(GetNavigationHierarchyResponse response, List<string> values)
        {
            this.response = response;
            this.values = values;
        }

        public bool Validate()
        {
            return true;
        }

        public bool Bind()
        {
            AddLocations(response.Hierarchy.ViewPoints);
            return true;
        }

        private void AddLocations(IEnumerable<ViewPoint> viewPoints)
        {
            if (viewPoints != null)
            {
                foreach (var viewPoint in viewPoints)
                {
                    if (viewPoint.ReportingPoints != null)
                    {
                        foreach (var reportingPoint in viewPoint.ReportingPoints)
                        {
                            values.Add(reportingPoint.id);
                        }
                    }
                    AddLocations(viewPoint.ViewPoints);
                }
            }
        }
    }
}
