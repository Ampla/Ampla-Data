using System;
using AmplaData.Attributes;

namespace AmplaData.Modules.Downtime
{
    [AmplaLocation(Location = "Enterprise.Site.Area.Downtime")]
    [AmplaModule(Module = "Downtime")]
    public class SimpleDowntimeModel
    {
        public int Id { get; set; }
        public string Location { get; set; }

        [AmplaField(Field = "Start Time")]
        public DateTime StartTime { get; set; }

        public int Duration { get; set; }

        [AmplaField(Field = "Cause Location")]
        public string CauseLocation { get; set; }

        public string Cause { get; set; }

        public string Classification { get; set; }
    }
}