using System;
using AmplaWeb.Data.Attributes;

namespace AmplaWeb.Sample.Models
{
    [AmplaLocation(Location="Enterprise", WithRecurse = true)]
    [AmplaModule(Module = "Production")]
    public class ProductionModel
    {
        public int Id { get; set; }

        [AmplaField(Field = "Sample Period")]
        public DateTime Sample { get; set; }

        public string Location { get; set; }
    }
}