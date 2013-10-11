using System;
using AmplaWeb.Data.Attributes;

namespace AmplaWeb.Sample.Models
{
    [AmplaLocation(Location="Enterprise", WithRecurse = true)]
    [AmplaModule(Module = "Quality")]
    public class QualityModel
    {
        public int Id { get; set; }

        [AmplaField(Field = "Sample Period")]
        public DateTime Sample { get; set; }

        public int Duration { get; set; }

        public string Location { get; set; }
    }
}