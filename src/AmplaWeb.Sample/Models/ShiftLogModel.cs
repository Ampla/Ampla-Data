﻿using System.ComponentModel;
using AmplaData.Data.Attributes;

namespace AmplaData.Web.Sample.Models
{
    [DisplayName("Shift Log")]
    [AmplaLocation(Location = "Enterprise.Site.Area.Knowledge")]
    [AmplaModule(Module = "Knowledge")]
    public class ShiftLogModel
    {
        public int Id { get; set; }

        public string Comments { get; set; }
        
        [ReadOnly(true)]
        public string Shift { get; set; }
    }
}