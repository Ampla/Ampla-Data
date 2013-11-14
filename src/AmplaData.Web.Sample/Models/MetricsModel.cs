using System;
using AmplaData.Attributes;

namespace AmplaData.Web.Sample.Models
{
    [AmplaLocation(Location="Enterprise", WithRecurse = true)]
    [AmplaModule(Module = "Metrics")]
    [AmplaDefaultFilters("@GroupBy={Hour}", "Sample Period={Current Day}")]
    public class MetricsModel
    {
        public int Id { get; set; }

        [AmplaField(Field = "Start Time")]
        public DateTime StartTime { get; set; }

        [AmplaField(Field = "End Time")]
        public DateTime EndTime { get; set; }

        public string Location { get; set; }

        public string Period { get; set; }
    }
}