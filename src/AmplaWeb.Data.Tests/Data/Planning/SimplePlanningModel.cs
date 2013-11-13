using System;
using AmplaData.Data.Attributes;

namespace AmplaData.Data.Planning
{
    [AmplaLocation(Location = "Enterprise.Site.Area.Planning")]
    [AmplaModule(Module = "Planning")]
    public class SimplePlanningModel
    {
        public int Id { get; set; }

        public string Location { get; set; }

        [AmplaField(Field = "Planned Start Time")]
        public DateTime PlannedStart { get; set; }

        [AmplaField(Field = "Planned End Time")]
        public DateTime PlannedEnd { get; set; }

        public string ActivityId { get; set; }

        public string State { get; set; }
    }
}