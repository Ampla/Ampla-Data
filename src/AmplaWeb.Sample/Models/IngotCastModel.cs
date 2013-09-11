using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AmplaWeb.Data.Attributes;

namespace AmplaWeb.Sample.Models
{
    [DisplayName("Ingot Cast")]
    [AmplaLocation(Location = "Enterprise.Site.Area.Production")]
    [AmplaModule(Module = "Production")]
    public class IngotCastModel
    {
        public int Id { get; set; }
        [Required]
        public string CastNo { get; set; }
    }

    public class IngotCastWithBundlesModel
    {
        public IngotCastWithBundlesModel(IngotCastModel cast)
        {
            Cast = cast;
            Bundles = new List<IngotBundleModel>();
        }
        
        public IngotCastModel Cast { get; private set; }
        public List<IngotBundleModel> Bundles { get; set; }

        public int Id 
        { get { return Cast.Id; } 
            set {throw new NotImplementedException();}
        }
    }
}