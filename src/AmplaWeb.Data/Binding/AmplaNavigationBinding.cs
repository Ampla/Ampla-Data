using System;
using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding
{
    public class AmplaNavigationBinding<TModel> : IAmplaBinding where TModel : class, new()
    {
        private readonly GetNavigationHierarchyResponse response;
        private readonly List<string> values;
        private readonly IModelProperties<TModel> modelProperties;

        public AmplaNavigationBinding(GetNavigationHierarchyResponse response, List<string> values,
                                      IModelProperties<TModel> modelProperties)
        {
            this.response = response;
            this.values = values;
            this.modelProperties = modelProperties;
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
