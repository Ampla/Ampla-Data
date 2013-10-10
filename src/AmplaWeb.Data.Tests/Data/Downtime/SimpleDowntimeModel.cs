using System;
using AmplaWeb.Data.Attributes;

namespace AmplaWeb.Data.Downtime
{
    [AmplaLocation(Location = "Enterprise.Site.Area.Point")]
    [AmplaModule(Module = "Downtime")]
    public class SimpleDowntimeModel
    {
        public int Id { get; set; }
        public string Location { get; set; }

        [AmplaField(Field = "Start Time")]
        public DateTime StartTime { get; set; }

        [AmplaField(Field = "Cause Location")]
        public string CauseLocation { get; set; }

        public string Cause { get; set; }

        public string Classification { get; set; }
    }
}