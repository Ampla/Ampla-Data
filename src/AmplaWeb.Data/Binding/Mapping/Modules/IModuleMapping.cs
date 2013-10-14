using AmplaWeb.Data.Binding.ViewData;

namespace AmplaWeb.Data.Binding.Mapping.Modules
{
    public interface IModuleMapping
    {
        /// <summary>
        /// Gets the field mapping for the specified view field
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="isModelField">if set to <c>true</c> [is model field].</param>
        /// <returns></returns>
        FieldMapping GetFieldMapping(ViewField field, bool isModelField);

        /// <summary>
        /// Gets the module permissions that determine what operations are allowed per module
        /// </summary>
        /// <returns></returns>
        IViewPermissions GetModulePermissions();
    }
}