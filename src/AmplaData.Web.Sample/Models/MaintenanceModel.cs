using System;
using AmplaData.Data.Attributes;

namespace AmplaData.Web.Sample.Models
{
    [AmplaLocation(Location="Enterprise", WithRecurse = true)]
    [AmplaModule(Module = "Maintenance")]
    public class MaintenanceModel
    {
        public int Id { get; set; }

        [AmplaField(Field = "Sample Period")]
        public DateTime Sample { get; set; }

        public TimeSpan Duration { get; set; }

        public string Location { get; set; }
    }
}