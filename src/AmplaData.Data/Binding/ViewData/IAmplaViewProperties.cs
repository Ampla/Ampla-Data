using System.Collections.Generic;
using AmplaData.Binding.Mapping;

namespace AmplaData.Binding.ViewData
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

        /// <summary>
        /// Update the model using the field's Display Name
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="useDisplayName">if set to <c>true</c> [use display name].</param>
        void UpdateModel(TModel model, string name, string value, bool useDisplayName);

        /// <summary>
        /// Gets the display name of the field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        string GetFieldDisplayName(string fieldName);
    }
}