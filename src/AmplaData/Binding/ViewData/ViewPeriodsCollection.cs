﻿using AmplaData.AmplaData2008;

namespace AmplaData.Binding.ViewData
{
    public class ViewPeriodsCollection : ViewCollection<ViewPeriod>
    {
        public void Initialise(GetView view)
        {
            if (view.Periods != null)
            {
                foreach (GetViewsPeriod period in view.Periods)
                {
                    ViewPeriod viewPeriod = new ViewPeriod(period);
                    Add(viewPeriod.Name, viewPeriod);
                }
            }
        }
    }
}