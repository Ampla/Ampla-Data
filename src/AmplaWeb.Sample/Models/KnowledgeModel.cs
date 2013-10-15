using System;
using AmplaWeb.Data.Attributes;

namespace AmplaWeb.Sample.Models
{
    [AmplaLocation(Location="Enterprise", WithRecurse = true)]
    [AmplaModule(Module = "Knowledge")]
    public class KnowledgeModel
    {
        public int Id { get; set; }

        [AmplaField(Field = "Sample Period")]
        public DateTime SamplePeriod { get; set; }

        public TimeSpan Duration { get; set; }

        public string Location { get; set; }

        public bool Confirmed { get; set; }
    }
}