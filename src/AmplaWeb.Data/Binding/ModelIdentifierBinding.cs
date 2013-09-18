using System;
using System.Collections.Generic;
using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding
{
    public class ModelIdentifierBinding<TModel> : IAmplaBinding where TModel : class, new()
    {
        private readonly TModel model;
        private readonly List<int> identifiers;
        private readonly IModelProperties<TModel> modelProperties;

        public ModelIdentifierBinding(TModel model, List<int> identifiers, IModelProperties<TModel> modelProperties)
        {
            this.model = model;
            this.identifiers = identifiers;
            this.modelProperties = modelProperties;
        }

        public bool Bind()
        {
            if (model == null) return false;

            string idProperty = ModelIdentifier.GetPropertyName<TModel>();
            if (string.IsNullOrEmpty(idProperty))
            {
                return false;
            }

            string value;
            modelProperties.TryGetPropertyValue(model, idProperty, out value);
            identifiers.Add(Convert.ToInt32(value));

            return true;
        }
    }
}