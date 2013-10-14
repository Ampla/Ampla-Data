using System;
using AmplaWeb.Data.Attributes;

namespace AmplaWeb.Data.Energy
{
    [AmplaLocation(Location = "Enterprise.Site.Area.Energy")]
    [AmplaModule(Module = "Energy")]
    public class IdentifierEnergyModel
    {
        public int Id { get; set; }
        public string Location { get; set; }

        [AmplaField(Field = "Start Time")]
        public DateTime StartTime { get; set; }

        [AmplaField(Field = "Cause Location")]
        public string CauseLocation { get; set; }

        /// <summary>
        /// Uso
        /// </summary>
        public int Cause { get; set; }

        public int Classification { get; set; }
    }
}