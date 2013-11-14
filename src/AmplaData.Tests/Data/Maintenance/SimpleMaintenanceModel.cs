using System;
using AmplaData.Attributes;

namespace AmplaData.Maintenance
{
    [AmplaLocation(Location = "Enterprise.Site.Area.Maintenance")]
    [AmplaModule(Module = "Maintenance")]
    public class SimpleMaintenanceModel
    {
        public int Id { get; set; }

        public string Location { get; set; }

        [AmplaField(Field = "Sample Period")]
        public DateTime SamplePeriod { get; set; }

        public int Duration { get; set; }
    }
}