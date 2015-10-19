using System;
using AmplaData.Binding.ModelData;

namespace AmplaData.Binding.Mapping
{
    public class DefaultValueFieldMapping<T> : ModelFieldMapping
    {
        private readonly Func<string> defaultValue;

        public DefaultValueFieldMapping(string name, Func<string> defaultValue) : base(name, typeof(T))
        {
            this.defaultValue = defaultValue;
        }

        public override bool TryResolveValue<TModel>(IModelProperties<TModel> modelProperties, TModel model, out string value)
        {
            string baseValue;
            if (base.TryResolveValue(modelProperties, model, out baseValue))
            {
                if (!modelProperties.IsDefaultValue(model, Name))
                {
                    value = baseValue;
                    return true;
                }
            }

            value = defaultValue();
            return true;
        }
    }
}