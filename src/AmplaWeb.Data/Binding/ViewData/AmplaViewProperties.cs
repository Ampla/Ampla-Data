using System;
using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.Mapping;
using AmplaWeb.Data.Binding.Mapping.Modules;
using AmplaWeb.Data.Binding.MetaData;
using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding.ViewData
{
    public class AmplaViewProperties<TModel> : IAmplaViewProperties where TModel : new()
    {
        private readonly IModelProperties<TModel> modelProperties;

        private readonly ViewPermissions permissions = new ViewPermissions();
        private readonly ViewFieldsCollection viewFieldsCollection = new ViewFieldsCollection();
        private readonly ViewFiltersCollection viewFiltersCollection = new ViewFiltersCollection();
        private readonly ViewPeriodsCollection viewPeriodsCollection = new ViewPeriodsCollection();
        private List<FieldMapping> fieldResolvers = new List<FieldMapping>();
        private readonly IViewPermissions enforcePermissions;

        public AmplaViewProperties( IModelProperties<TModel> modelProperties )
        {
            this.modelProperties = modelProperties;
            permissions = new ViewPermissions();
            IViewPermissions modulePermissions = ModuleMapping.GetModuleMapping(modelProperties.Module).GetModulePermissions();
            enforcePermissions = new EnforceViewPermissionsAdapter(modelProperties.Module.ToString(), permissions, modulePermissions);
        }

        public IViewPermissions Enforce
        {
            get { return enforcePermissions; }
        }

        public IViewPermissions Permissions
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

        public IEnumerable<FieldMapping> GetFieldMappings()
        {
            return fieldResolvers;
        }

        private List<FieldMapping> BuildFieldResolvers()
        {
            IModuleMapping moduleMapping = ModuleMapping.GetModuleMapping(modelProperties.Module);

            List<FieldMapping> fieldMappings = new List<FieldMapping>();
            List<string> modelFields = new List<string>(modelProperties.GetProperties());
            foreach (ViewField field in viewFieldsCollection.GetValues())
            {
                bool isModelProperty = modelFields.Contains(field.DisplayName);

                FieldMapping fieldMapping = moduleMapping.GetFieldMapping(field, isModelProperty);

                
                
                //if (isModelProperty)
                //{
                //    fieldMapping = moduleMapping.GetFieldMapping(field);
                //    if (fieldMapping == null)
                //    {
                //        if (field.ReadOnly)
                //        {
                //            fieldMapping = new ReadOnlyFieldMapping(field.DisplayName);
                //        }
                //        else
                //        {
                //            fieldMapping = new ModelFieldMapping(field.DisplayName);
                //        }
                //    }
                //}
                //else
                //{
                //    fieldMapping = moduleMapping.GetRequiredFieldMapping(field);
                //}
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

        private static bool IsValidId(string s)
        {
            int i;
            return !string.IsNullOrEmpty(s) && int.TryParse(s, out i) && i > 0; 
        }

        private FieldMapping GetSpecialFieldMapping(ViewField field)
        {
            if (field.Name == "Id")
            {
                return new IdFieldMapping("Id");
            }

            if (field.Name == "ObjectId")
            {
                return new ReadOnlyFieldMapping("Location");
            }

            if (field.Name == "SampleDateTime")
            {
                return new DefaultValueFieldMapping("Sample Period", Iso8601UtcNow);
            }

            if (field.Name == "Cause Location")
            {
                return new ValidatedModelFieldMapping(field.Name, s => !string.IsNullOrEmpty(s));
            }

            if (field.Name == "Cause")
            {
                return new ValidatedModelFieldMapping(field.Name, IsValidId);
            }

            if (field.Name == "Classification")
            {
                return new ValidatedModelFieldMapping(field.Name, IsValidId);
            }

            return null;
        }
    }
}