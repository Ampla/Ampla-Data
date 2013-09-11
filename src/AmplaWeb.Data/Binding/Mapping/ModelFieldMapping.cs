using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding.Mapping
{
    public class ModelFieldMapping : FieldMapping
    {
        public ModelFieldMapping(string name) : base(name)
        {
            CanRead = true;
            CanWrite = true;
        }

        public override bool TryResolveValue<TModel>(ModelProperties<TModel> modelProperties, TModel model, out string value)
        {
            return modelProperties.TryGetPropertyValue(model, Name, out value);
        }
    }
}