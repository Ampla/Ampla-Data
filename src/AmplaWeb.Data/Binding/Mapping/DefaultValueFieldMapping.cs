using System;
using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding.Mapping
{
    public class DefaultValueFieldMapping : ModelFieldMapping
    {
        private readonly Func<string> defaultValue;

        public DefaultValueFieldMapping(string name, Func<string> defaultValue) : base(name)
        {
            this.defaultValue = defaultValue;
        }

        public override bool TryResolveValue<TModel>(ModelProperties<TModel> modelProperties, TModel model, out string value)
        {
            if (!base.TryResolveValue(modelProperties, model, out value))
            {
                value = defaultValue();
            }
            return true;
        }
    }
}