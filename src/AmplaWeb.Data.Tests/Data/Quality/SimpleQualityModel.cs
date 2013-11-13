﻿using System;
using AmplaData.Data.Attributes;

namespace AmplaData.Data.Quality
{
    [AmplaLocation(Location = "Enterprise.Site.Area.Quality")]
    [AmplaModule(Module = "Quality")]
    public class SimpleQualityModel
    {
        public int Id { get; set; }

        public string Location { get; set; }

        [AmplaField(Field = "Sample Period")]
        public DateTime SamplePeriod { get; set; }

        public int Duration { get; set; }
    }
}