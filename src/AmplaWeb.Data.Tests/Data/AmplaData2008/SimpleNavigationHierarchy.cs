using System.Collections.Generic;
using System.Linq;

namespace AmplaWeb.Data.AmplaData2008
{
    public class SimpleNavigationHierarchy
    {
        private class Node
        {
            public Node(string name)
            {
                Name = name;
                Children = new List<Node>();
                IsReportingPoint = false;
            }

            private bool IsReportingPoint { get; set; }
            private string Id { get; set; }
            private string Name { get; set; }

            private List<Node> Children { get; set; }

            public void AddLocation(string location)
            {
                Node context = this;
                string[] parts = location.Split('.');
                for (int i = 0; i < parts.Length; i++)
                {
                    string part = parts[i];
                    Node child = context.Find(part);
                    if (child == null)
                    {
                        child = new Node(part) {Id = context.Id == null ? part : context.Id + "." + part};
                        context.Children.Add(child);
                    }
                    if (parts.Length == i + 1)
                    {
                        child.IsReportingPoint = true;
                    }
                    context = child;
                }
            }

            public ViewPoint[] GetViewPoints()
            {
                List<ViewPoint> folders = Children.Select(child => child.GetViewPoint()).ToList();
                return folders.ToArray();
            }

            private ViewPoint GetViewPoint()
            {
                List<GetNavigationReportingPoint> points = new List<GetNavigationReportingPoint>();
                List<ViewPoint> folders = new List<ViewPoint>();

                foreach (Node child in Children)
                {
                    if (child.IsReportingPoint)
                    {
                        points.Add(child.GetReportingPoint());
                    }
                    else
                    {
                        folders.Add(child.GetViewPoint());
                    }
                }
                return new ViewPoint
                    {
                        id = Id,
                        DisplayName = Name,
                        LocalizedDisplayName = Name,
                        ReportingPoints = points.ToArray(),
                        ViewPoints = folders.ToArray()
                    };
            }

            private GetNavigationReportingPoint GetReportingPoint()
            {
                return new GetNavigationReportingPoint
                    {
                        id = Id,
                        DisplayName = Name,
                        LocalizedDisplayName = Name
                    };
            }

            private Node Find(string name)
            {
                return Children.FirstOrDefault(child => child.Name == name);
            }
        }

        private readonly AmplaModules amplaModule;
        private readonly string[] locations;

        public SimpleNavigationHierarchy(AmplaModules amplaModule, string[] locations)
        {
            this.amplaModule = amplaModule;
            this.locations = locations;
        }

        public Hierarchy GetHierarchy()
        {
            Node rootNode = new Node(null);
            foreach (string location in locations)
            {
                rootNode.AddLocation(location);
            }

            Hierarchy hierarchy = new Hierarchy
            {
                module = amplaModule,
                context = NavigationContext.Plant,
                mode = NavigationMode.Location,
                ViewPoints = rootNode.GetViewPoints()
            };
            
            return hierarchy;
        }
    }
}