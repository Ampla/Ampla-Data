using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding.Mapping
{
    public class ConstantFieldMapping : FieldMapping
    {
        public ConstantFieldMapping(string name, string value) : base(name)
        {
            CanRead = true;
            CanWrite = false;

            Value = value;
        }

        public string Value { get; private set; }

        public override bool TryResolveValue<TModel>(ModelProperties<TModel> modelProperties, TModel model, out string value)
        {
            value = Value;
            return true;
        }
    }
}