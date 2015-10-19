using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.Binding.Mapping;
using AmplaData.Binding.Mapping.Modules;
using AmplaData.Binding.ViewData;
using AmplaData.Dynamic.Binding.ModelData;

namespace AmplaData.Dynamic.Binding.ViewData
{
    public class DynamicAmplaViewProperties : IDynamicAmplaViewProperties
    {
        private readonly IDynamicModelProperties modelProperties;

        private readonly ViewPermissions permissions = new ViewPermissions();
        private readonly ViewFieldsCollection viewFieldsCollection = new ViewFieldsCollection();
        private readonly ViewFiltersCollection viewFiltersCollection = new ViewFiltersCollection();
        private readonly ViewPeriodsCollection viewPeriodsCollection = new ViewPeriodsCollection();
        private List<FieldMapping> fieldResolvers = new List<FieldMapping>();
        private readonly IViewPermissions enforcePermissions;

        public DynamicAmplaViewProperties(IDynamicModelProperties modelProperties)
        {
            this.modelProperties = modelProperties;
            permissions = new ViewPermissions();
            IViewPermissions modulePermissions =
                ModuleMapping.GetModuleMapping(modelProperties.Module).GetSupportedOperations();
            enforcePermissions = new EnforceViewPermissionsAdapter(modelProperties.Module.ToString(), permissions,
                                                                   modulePermissions);
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

        /// <summary>
        /// Updates the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="useDisplayName">if set to <c>true</c> [use display name].</param>
        public void UpdateModel(object model, string name, string value, bool useDisplayName)
        {
            ViewField field = useDisplayName ? viewFieldsCollection.FindByDisplayName(name) : viewFieldsCollection.FindByName(name);
            if (field != null)
            {
                modelProperties.TrySetValueFromString(model, field.DisplayName, value);
            }
        }

        /// <summary>
        /// Gets the display name of the field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public string GetFieldDisplayName(string fieldName)
        {
            ViewField viewField = viewFieldsCollection.FindByName(fieldName);
            return viewField != null ? viewField.DisplayName : fieldName;
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

                if (fieldMapping != null)
                {
                    fieldMappings.Add(fieldMapping);
                }
            }
            return fieldMappings;
        }

        /// <summary>
        /// Validates the permissions for the view
        /// </summary>
        /// <returns></returns>
        public IList<string> ValidateViewPermissions()
        {
            IViewPermissions modulePermissions = ModuleMapping.GetModuleMapping(modelProperties.Module).GetSupportedOperations();
            ValidateViewPermissions validatePermissions = new ValidateViewPermissions(modelProperties.Module.ToString(), permissions,
                                                                   modulePermissions);
            validatePermissions.ValidatePermissions();

            return validatePermissions.Messages;
        }


        public bool ValidateRead(FieldMapping fieldMapping)
        {
            throw new System.NotImplementedException();
        }
    }
}