using System;
using AmplaData.Attributes;

namespace AmplaData.Modules.Metrics
{
    [AmplaLocation(Location = "Enterprise.Site.Area.Metrics")]
    [AmplaModule(Module = "Metrics")]
    public class SimpleMetricsModel
    {
        public int Id { get; set; }

        public string Location { get; set; }

        [AmplaField(Field = "Start Time")]
        public DateTime StartTime { get; set; }

        [AmplaField(Field = "End Time")]
        public DateTime EndTime { get; set; }

        public int Duration { get; set; }

        public string Period { get; set; }

        [AmplaField(Field = "Total Tonnes")]
        public double TotalTonnes { get; set; }
    }
}