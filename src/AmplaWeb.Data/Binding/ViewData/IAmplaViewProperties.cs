using System.Collections.Generic;
using AmplaWeb.Data.Binding.Mapping;

namespace AmplaWeb.Data.Binding.ViewData
{
    public interface IAmplaViewProperties<in TModel>
    {
        ViewPermissions Permissions { get; }
        IEnumerable<FieldMapping> GetFieldMappings();
    }
}