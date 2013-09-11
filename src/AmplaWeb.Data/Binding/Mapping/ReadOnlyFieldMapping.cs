using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding.Mapping
{
    public class ReadOnlyFieldMapping : FieldMapping
    {
        public ReadOnlyFieldMapping(string name) : base(name)
        {
            CanRead = true;
            CanWrite = false;
        }

        public override bool TryResolveValue<TModel>(ModelProperties<TModel> modelProperties, TModel model, out string value)
        {
            throw new System.NotImplementedException();
        }
    }
}