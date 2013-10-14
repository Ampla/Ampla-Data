using System;
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

        protected IModuleMapping moduleMapping { get; private set; }
        protected ViewFieldsCollection viewFields { get; private set; }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            moduleMapping = moduleMappingFunc();
            GetView view = getViewsFunc();

            viewFields = new ViewFieldsCollection();
            viewFields.Initialise(view);
        }

        protected void CheckField<T>(string name, string displayName, bool specialField, bool requiredField)
        {
            ViewField field = viewFields.Find(name);
            Assert.That(field, Is.Not.Null, "Unabled to find field: {0}", name);

            FieldMapping specialFieldMapping = moduleMapping.GetFieldMapping(field, true);
            FieldMapping requiredFieldMapping = moduleMapping.GetFieldMapping(field, false);
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


    }
}