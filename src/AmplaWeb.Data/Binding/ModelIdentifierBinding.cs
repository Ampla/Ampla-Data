using System;
using System.Collections.Generic;
using AmplaData.Data.Binding.ModelData;

namespace AmplaData.Data.Binding
{
    public class ModelIdentifierBinding<TModel> : IAmplaBinding where TModel : class, new()
    {
        private readonly TModel model;
        private readonly List<int> identifiers;
        private readonly IModelProperties<TModel> modelProperties;
        private readonly string idProperty;

        public ModelIdentifierBinding(TModel model, List<int> identifiers, IModelProperties<TModel> modelProperties)
        {
            this.model = model;
            this.identifiers = identifiers;
            this.modelProperties = modelProperties;
            idProperty = ModelIdentifier.GetPropertyName<TModel>();
        }

        public bool Bind()
        {
            if (model == null) return false;

            if (string.IsNullOrEmpty(idProperty))
            {
                return false;
            }

            string value;
            modelProperties.TryGetPropertyValue(model, idProperty, out value);
            identifiers.Add(Convert.ToInt32(value));

            return true;
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(idProperty);
        }
    }
}