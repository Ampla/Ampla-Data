using System;
using AmplaData.Attributes;

namespace AmplaData.Energy
{
    [AmplaLocation(Location = "Enterprise.Site.Area.Energy")]
    [AmplaModule(Module = "Energy")]
    public class SimpleEnergyModel
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