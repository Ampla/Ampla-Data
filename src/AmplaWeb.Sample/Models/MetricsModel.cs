using System;
using AmplaWeb.Data.Attributes;

namespace AmplaWeb.Sample.Models
{
    [AmplaLocation(Location="Enterprise", WithRecurse = true)]
    [AmplaModule(Module = "Metrics")]
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