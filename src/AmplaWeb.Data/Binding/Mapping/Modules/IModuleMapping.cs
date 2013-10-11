using AmplaWeb.Data.Binding.ViewData;

namespace AmplaWeb.Data.Binding.Mapping.Modules
{
    public interface IModuleMapping
    {
        FieldMapping GetFieldMapping(ViewField field, bool isModelField);
    }
}