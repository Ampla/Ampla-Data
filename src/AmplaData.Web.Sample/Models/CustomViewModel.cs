using System;
using AmplaData.Attributes;

namespace AmplaData.Web.Sample.Models
{
    [AmplaLocation(Location = "Enterprise.Site.Area.CustomView")]
    [AmplaModule(Module = "Quality")]
    public class CustomViewModel
    {
        [AmplaField(Field = "Sample Period")]
        public DateTime Day { get; set; }

        [AmplaField("RecordCount")]
        public int RecordCount { get; set; }

        [AmplaField("Average")]
        public double Average { get; set; }
    }
}