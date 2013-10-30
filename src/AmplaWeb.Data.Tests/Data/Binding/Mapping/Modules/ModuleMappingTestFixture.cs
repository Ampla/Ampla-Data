using System;
using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.ViewData;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.Mapping.Modules
{
    [TestFixture]
    public abstract class ModuleMappingTestFixture : TestFixture
    {
        private readonly Func<GetView> getViewsFunc;
        private readonly Func<IModuleMapping> moduleMappingFunc;

        protected ModuleMappingTestFixture(Func<GetView> getViewsFunc, Func<IModuleMapping> moduleMappingFunc )
        {
            this.getViewsFunc = getViewsFunc;
            this.moduleMappingFunc = moduleMappingFunc;
        }

        private IModuleMapping ModuleMapping { get; set; }
        private ViewFieldsCollection ViewFields { get; set; }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            ModuleMapping = moduleMappingFunc();
            GetView view = getViewsFunc();

            ViewFields = new ViewFieldsCollection();
            ViewFields.Initialise(view);
        }

        protected void CheckField<T>(string name, string displayName, bool specialField, bool requiredField)
        {
            ViewField field = ViewFields.FindByName(name);
            Assert.That(field, Is.Not.Null, "Unabled to find field: {0}", name);

            FieldMapping specialFieldMapping = ModuleMapping.GetFieldMapping(field, true);
            FieldMapping requiredFieldMapping = ModuleMapping.GetFieldMapping(field, false);
            if (specialField)
            {
                Assert.That(specialFieldMapping, Is.Not.Null);
                Assert.That(specialFieldMapping, Is.TypeOf<T>());
                Assert.That(specialFieldMapping.Name, Is.EqualTo(displayName));
            }
            else
            {
                Assert.That(specialFieldMapping, Is.Null);
            }

            if (requiredField)
            {
                Assert.That(requiredFieldMapping, Is.Not.Null);
                Assert.That(requiredFieldMapping, Is.TypeOf<T>());
                Assert.That(requiredFieldMapping.Name, Is.EqualTo(displayName));
            }
            else
            {
                Assert.That(requiredFieldMapping, Is.Null);
            }
        }

        protected void CheckAllowedOperations(params ViewAllowedOperations[] operations)
        {
            IViewPermissions supportedOperations = ModuleMapping.GetSupportedOperations();

            List<ViewAllowedOperations> allowedOperations = new List<ViewAllowedOperations>();
            allowedOperations.AddRange(operations);

            Assert.That(supportedOperations.CanAdd(), Is.EqualTo(allowedOperations.Contains(ViewAllowedOperations.AddRecord)), "AddRecord");
            Assert.That(supportedOperations.CanConfirm(), Is.EqualTo(allowedOperations.Contains(ViewAllowedOperations.ConfirmRecord)), "ConfirmRecord");
            Assert.That(supportedOperations.CanDelete(), Is.EqualTo(allowedOperations.Contains(ViewAllowedOperations.DeleteRecord)), "DeleteRecord");
            Assert.That(supportedOperations.CanModify(), Is.EqualTo(allowedOperations.Contains(ViewAllowedOperations.ModifyRecord)), "ModifyRecord");
            Assert.That(supportedOperations.CanSplit(), Is.EqualTo(allowedOperations.Contains(ViewAllowedOperations.SplitRecord)), "SplitRecord");
            Assert.That(supportedOperations.CanUnconfirm(), Is.EqualTo(allowedOperations.Contains(ViewAllowedOperations.UnconfirmRecord)), "UnconfirmRecord");
            Assert.That(supportedOperations.CanView(), Is.EqualTo(allowedOperations.Contains(ViewAllowedOperations.ViewRecord)), "ViewRecord");
        }
    }
}