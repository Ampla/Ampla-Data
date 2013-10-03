using System.Collections.Generic;
using AmplaWeb.Data.Binding.Mapping;

namespace AmplaWeb.Data.Binding.ViewData
{
    public interface IAmplaViewProperties<in TModel>
    {
        /// <summary>
        ///     Gets the Permissions for the view
        /// </summary>
        IViewPermissions Enforce { get; }

        
        /// <summary>
        ///     Gets the Permissions for the view
        /// </summary>
        IViewPermissions Permissions { get; }
        
        /// <summary>
        ///     Gets the Field Mappings for the view
        /// </summary>
        /// <returns></returns>
        IEnumerable<FieldMapping> GetFieldMappings();
    }
}