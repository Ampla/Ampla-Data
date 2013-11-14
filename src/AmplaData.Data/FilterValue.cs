using System;

namespace AmplaData
{
    public class FilterValue
    {
        public FilterValue()
        {
            
        }

        public FilterValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// Tries to Parse the filter value from a string
        /// </summary>
        /// <param name="filterString">The filter string.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static bool TryParse(string filterString, out FilterValue filter)
        {
            filter = null;

            if (filterString != null)
            {
                filter = ParseFullFormat(filterString) ?? ParseSimpleFormat(filterString);
            }
            return filter != null;
        }

        /// <summary>
        /// Parses the specified filter string.
        /// </summary>
        /// <param name="filterString">The filter string.</param>
        /// <returns></returns>
        public static FilterValue Parse(string filterString)
        {
            FilterValue filterValue = null;
            if (!string.IsNullOrEmpty(filterString))
            {
                filterValue = ParseFullFormat(filterString) ?? ParseSimpleFormat(filterString);
            }
            if (filterValue == null)
            {
                throw new ArgumentException("FilterValue format is not valid.  Available formats are: \r\nFilter=Value or Filter={Value}");
            }
            return filterValue;
        }

        private static FilterValue ParseSimpleFormat(string filterString)
        {
            string[] parts = filterString.Split(new[] { "=" }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                string filter = parts[0].Trim();
                string value = parts[1].Trim();

                if (!string.IsNullOrEmpty(filter))
                {
                    return new FilterValue(filter, value);
                }
            }
            return null;
        }

        private static FilterValue ParseFullFormat(string filterString)
        {
            string[] parts = filterString.Split(new []{"={"}, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                if (parts[1].EndsWith("}"))
                {
                    string filter = parts[0].Trim();
                    string value = parts[1].Substring(0, parts[1].Length - 1).Trim();

                    if (!string.IsNullOrEmpty(filter))
                    {
                        return new FilterValue(filter, value);
                    }
                }
            }
            return null;
        }
        
        public override string ToString()
        {
            return string.Format("FilterValue ({0} = {1})", Name, Value);
        }
    }
}