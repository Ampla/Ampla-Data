using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AmplaWeb.Data.Attributes;

namespace AmplaWeb.Sample.Models
{

    [DisplayName("Ingot Bundles")]
    [AmplaLocation(Location="Enterprise.Site.Area.Bundles")]
    [AmplaModule(Module="Production")]
    public class IngotBundleModel 
    {
        public int Id { get; set; }
        [Required]
        public string CastNo { get; set; }

        [Required]
        public string BundleNo { get; set; }
        public int Length { get; set; }
        public int Diameter { get; set; }
        public int NetWeight { get; set; }
        public int GrossWeight { get; set; }
        public int Bow { get; set; }
        public int NoOfStraps { get; set; }
        public string SurfaceFinish { get; set; }
        public string InspectedBy { get; set; }
        public string Remarks { get; set; }
        public string SuggestedGrade { get; set; }
        public string Status { get; set;  }
    }
}