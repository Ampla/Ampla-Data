using System;
using System.Collections.Generic;
using AmplaData.Data.Binding.MetaData;

namespace AmplaData.Data.Attributes
{
    /// <summary>
    ///     Default Filters Attribute used to define the default filters
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AmplaDefaultFiltersAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaDefaultFiltersAttribute"/> class.
        /// </summary>
        public AmplaDefaultFiltersAttribute() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaDefaultFiltersAttribute"/> class.
        /// </summary>
        public AmplaDefaultFiltersAttribute(params string[] filterAssignments)
        {
            List<FilterValue> filters = new List<FilterValue>();
            if (filterAssignments != null)
            {
                foreach (string filterAssignment in filterAssignments)
                {
                    FilterValue filter;
                    if (FilterValue.TryParse(filterAssignment, out filter))
                    {
                        filters.Add(filter);
                    }
                }
            }
            Filters = filters.ToArray();
        }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        private FilterValue[] Filters { get; set; }

        /// <summary>
        /// Tries the get filter specified
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="filterValues">The filter values.</param>
        /// <returns></returns>
        public static bool TryGetFilter<TModel>(out FilterValue[] filterValues)
        {
            AmplaDefaultFiltersAttribute attribute;
            if (typeof (TModel).TryGetAttribute(out attribute))
            {
                filterValues = attribute.Filters;
                return filterValues.Length > 0;
            }
            filterValues = new FilterValue[0];
            return false;
        }
    }
}