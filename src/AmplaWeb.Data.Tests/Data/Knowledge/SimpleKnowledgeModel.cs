using System;
using AmplaWeb.Data.Attributes;

namespace AmplaWeb.Data.Knowledge
{
    [AmplaLocation(Location = "Enterprise.Site.Area.Knowledge")]
    [AmplaModule(Module = "Knowledge")]
    public class SimpleKnowledgeModel
    {
        public int Id { get; set; }

        public string Location { get; set; }

        [AmplaField(Field = "Sample Period")]
        public DateTime SamplePeriod { get; set; }

        public int Duration { get; set; }
    }
}