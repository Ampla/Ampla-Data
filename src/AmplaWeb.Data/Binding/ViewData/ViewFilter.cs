using System;
using System.ComponentModel;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.MetaData;

namespace AmplaWeb.Data.Binding.ViewData
{
    public class ViewFilter
    {

        public ViewFilter(GetViewsFilter filter)
        {
            Name = filter.name;
            DisplayName = filter.displayName;

            DataType = DataTypeHelper.GetDataType(filter.type);
            TypeConverter = TypeDescriptor.GetConverter(DataType);
        }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public Type DataType { get; set; }

        public TypeConverter TypeConverter { get; set; }
    }
}