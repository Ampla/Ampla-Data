using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding.Mapping
{
    public abstract class FieldMapping
    {
        protected FieldMapping(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public bool CanRead
        {
            get; protected set;
        }

        public bool CanWrite
        {
            get; protected set;
        }

        public abstract bool TryResolveValue<TModel>(ModelProperties<TModel> modelProperties, TModel model, out string value) where TModel : new();
    }
}