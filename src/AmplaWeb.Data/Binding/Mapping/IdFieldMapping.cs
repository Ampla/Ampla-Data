using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding.Mapping
{
    public class IdFieldMapping : FieldMapping
    {
        public IdFieldMapping(string name) : base(name)
        {
           
        }

        public override bool TryResolveValue<TModel>(ModelProperties<TModel> modelProperties, TModel model, out string value)
        {
            bool resolved = modelProperties.TryGetPropertyValue(model, Name, out value);
            if (resolved && (value == "0"))
            {
                value = null;
                return false;
            }
            return resolved;
        }
    }
}