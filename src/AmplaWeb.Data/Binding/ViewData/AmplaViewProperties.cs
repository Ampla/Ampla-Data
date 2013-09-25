using System;
using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.Mapping;
using AmplaWeb.Data.Binding.MetaData;
using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding.ViewData
{
    public class AmplaViewProperties<TModel> : IAmplaViewProperties<TModel> where TModel : new()
    {
        private readonly IModelProperties<TModel> modelProperties;

        private readonly ViewPermissions permissions = new ViewPermissions();
        private readonly ViewFieldsCollection viewFieldsCollection = new ViewFieldsCollection();
        private readonly ViewFiltersCollection viewFiltersCollection = new ViewFiltersCollection();
        private readonly ViewPeriodsCollection viewPeriodsCollection = new ViewPeriodsCollection();
        private List<FieldMapping> fieldResolvers = new List<FieldMapping>(); 

        public AmplaViewProperties( IModelProperties<TModel> modelProperties )
        {
            this.modelProperties = modelProperties;
        }

        public ViewPermissions Permissions
        {
            get { return permissions; }
        }

        public void Initialise(GetViewsResponse response)
        {
            GetView view = response.Views[0];
            permissions.Initialise(view.AllowedOperations);
            viewFieldsCollection.Initialise(view);
            viewFiltersCollection.Initialise(view);
            viewPeriodsCollection.Initialise(view);
            fieldResolvers = BuildFieldResolvers();
        }
        
        public string GetPropertyValue(TModel model, string property)
        {
            string value;
            if (modelProperties.TryGetPropertyValue(model, property, out value))
            {
                
            }
            throw new NotImplementedException();
        }

        public List<string> GetFields()
        {
            List<string> properties = new List<string>(modelProperties.GetProperties());
            if (!properties.Contains("Location"))
            {
                properties.Add("Location");
            }
            return properties;
        }

        public IEnumerable<FieldMapping> GetFieldMappings()
        {
            return fieldResolvers;
        }

        private List<FieldMapping> BuildFieldResolvers()
        {
            List<FieldMapping> fieldMappings = new List<FieldMapping>();
            List<string> modelFields = new List<string>(modelProperties.GetProperties());
            foreach (ViewField field in viewFieldsCollection.GetValues())
            {
                FieldMapping fieldMapping;

                bool isModelProperty = modelFields.Contains(field.DisplayName);

                if (isModelProperty)
                {
                    fieldMapping = GetSpecialFieldMapping(field);
                    if (fieldMapping == null)
                    {
                        if (field.ReadOnly)
                        {
                            fieldMapping = new ReadOnlyFieldMapping(field.DisplayName);
                        }
                        else
                        {
                            fieldMapping = new ModelFieldMapping(field.DisplayName);
                        }
                    }
                }
                else
                {
                    fieldMapping = GetRequiredFieldMapping(field);
                }
                if (fieldMapping != null)
                {
                    fieldMappings.Add(fieldMapping);
    
                }
            }
            return fieldMappings;
        }

        private FieldMapping GetRequiredFieldMapping(ViewField field)
        {
            if (field.Name == "SampleDateTime")
            {
                return new DefaultValueFieldMapping("Sample Period", Iso8601UtcNow);
            }
            return null;
        }

        private static string Iso8601UtcNow()
        {
            return new Iso8601DateTimeConverter().ConvertToInvariantString(DateTime.UtcNow);
        }

        private FieldMapping GetSpecialFieldMapping(ViewField field)
        {
            if (field.Name == "Id")
            {
                return new IdFieldMapping("Id");
            }

            if (field.Name == "ObjectId")
            {
                return new LocationFieldMapping("Location", modelProperties.FilterLocation);
            }

            if (field.Name == "SampleDateTime")
            {
                return new DefaultValueFieldMapping("Sample Period", Iso8601UtcNow);
            }
            return null;
        }
    }
}