﻿using System;
using System.Collections.Generic;
using System.Linq;
using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Records.Filters
{
    public class InMemoryFilterMatcher : FilterMatcher
    {
        private readonly List<FilterMatcher> filters;

        public InMemoryFilterMatcher(DataFilter dataFilter)
        {
            filters = new List<FilterMatcher>();
            if (!string.IsNullOrEmpty(dataFilter.Location))
            {
                if (dataFilter.Location.EndsWith(" with recurse"))
                {
                    filters.Add(new LocationWithRecurseFilterMatcher(dataFilter.Location));
                }
                else
                {
                    filters.Add(new LocationFilterMatcher(dataFilter.Location));
                }
            }

            if (!string.IsNullOrEmpty(dataFilter.Deleted))
            {
                filters.Add(new FieldFilterMatcher<bool>("Deleted", dataFilter.Deleted));
            }

            if (!string.IsNullOrEmpty(dataFilter.SamplePeriod))
            {
                filters.Add(new FieldFilterMatcher<DateTime>("Sample Period", dataFilter.SamplePeriod));
            }
            if (dataFilter.Criteria != null)
            {
                foreach (FilterEntry entry in dataFilter.Criteria)
                {
                    if (entry.Name == "Id")
                    {
                        filters.Add(new IdFilterMatcher(entry.Value));
                    }
                    else
                    {
                        filters.Add(new FieldFilterMatcher<string>(entry.Name, entry.Value));
                    }
                }
            }
        }

        public override bool Matches(InMemoryRecord record)
        {
            if (filters.Any(filter => !filter.Matches(record)))
            {
                return false;
            }

            return filters.Count > 0;
        }
    }
}