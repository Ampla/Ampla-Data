using System;
using System.ComponentModel.DataAnnotations;
using AmplaData.Data.Attributes;

namespace AmplaData.Web.Sample.Models
{
    [AmplaLocation(Location = "Enterprise", WithRecurse = true)]
    [AmplaModule(Module = "Energy")]
    public class EnergyModel
    {
        public int Id { get; set; }

        [AmplaField(Field = "Start Time")]
        public DateTime StartTime { get; set; }

        [AmplaField(Field = "End Time")]
        public DateTime? EndTime { get; set; }

        public TimeSpan Duration { get; set; }

        [Required]
        public string Location { get; set; }

        [AmplaField(Field="Cause Location")]
        public string CauseLocation { get; set; }
        public string Classification { get; set; }
        public string Cause { get; set; }

        public string Comments { get; set; }
    }
}