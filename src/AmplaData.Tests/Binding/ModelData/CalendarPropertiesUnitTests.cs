using System;
using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.Attributes;
using NUnit.Framework;

namespace AmplaData.Binding.ModelData
{
    [TestFixture]
    [Ignore("Calendar still to be implemented")]
    public class CalendarPropertiesUnitTests : TestFixture
    {
        [AmplaLocation(Location="Enterprise.Site.Area.Simple")]
        [AmplaModule(Module="Production")]
        public class SimpleModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Value { get; set; }
        }

        [AmplaLocation(Location = "Enterprise.Site", WithRecurse = true )]
        [AmplaModule(Module = "Production")]
        public class SimpleModelWithRecurse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Value { get; set; }
        }


        public class ModelWithReadOnly : SimpleModel
        {
            public string ReadOnlyName
            {
                get { return Name; }
            }
        }

        public class ModelWithWriteOnly : SimpleModel
        {
            public string WriteOnlyName
            {
                set { Name = value; }
            }
        }

        public class InheritedModel : SimpleModel
        {
        }
        
        public class EmptyModel
        {
        }

        [Test]
        public void TestSimpleModel()
        {
            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            IList<string> properties = modelProperties.GetProperties();
            Assert.That(properties, Contains.Item("Id").And.Contains("Name").And.Contains("Value"));
            Assert.That(properties.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestInheritedModel()
        {
            CalendarProperties<InheritedModel> modelProperties = new CalendarProperties<InheritedModel>();

            IList<string> properties = modelProperties.GetProperties();
            Assert.That(properties, Contains.Item("Id").And.Contains("Name").And.Contains("Value"));
            Assert.That(properties.Count, Is.EqualTo(3));
        }


        [Test]
        public void ConstructorThrowsWhenNoAttributes()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(()=>new CalendarProperties<EmptyModel>());
            Assert.That(exception.Message, Is.StringContaining(typeof(AmplaLocationAttribute).Name));
            Assert.That(exception.Message, Is.StringContaining(typeof(AmplaModuleAttribute).Name));
        }

        [Test]
        public void TryGetPropertyWithDefaultValues()
        {
            SimpleModel model = new SimpleModel();

            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            AssertPropertyGetValue(modelProperties, model, "Id", "0");
            AssertPropertyGetValue(modelProperties, model, "Name", null);
            AssertPropertyGetValue(modelProperties, model, "Value", "0");
        }

        [Test]
        public void TryGetPropertyWithValidValues()
        {
            SimpleModel model = new SimpleModel {Id = 100, Name = "Ampla", Value = 1.234};

            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            AssertPropertyGetValue(modelProperties, model, "Id", "100");
            AssertPropertyGetValue(modelProperties, model, "Name", "Ampla");
            AssertPropertyGetValue(modelProperties, model, "Value", "1.234");
        }

        [Test]
        public void TryGetPropertyWithEmptyValues()
        {
            SimpleModel model = new SimpleModel { Id = 0, Name = "", Value = 0D };

            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            AssertPropertyGetValue(modelProperties, model, "Id", "0");
            AssertPropertyGetValue(modelProperties, model, "Name", "");
            AssertPropertyGetValue(modelProperties, model, "Value", "0");
        }

        [Test]
        public void TryGetPropertyWithInvalidProperty()
        {
            SimpleModel model = new SimpleModel();

            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            AssertPropertyNotGetValue(modelProperties, model, "InvalidId");
            AssertPropertyNotGetValue(modelProperties, model, "InvalidName");
            AssertPropertyNotGetValue(modelProperties, model, "InvalidValue");
        }

        [Test]
        public void TrySetPropertyWithValidValues()
        {
            SimpleModel model = new SimpleModel();

            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            AssertPropertySetValue(modelProperties, model, "Id", "100");
            AssertPropertySetValue(modelProperties, model, "Name", "Ampla");
            AssertPropertySetValue(modelProperties, model, "Value", "1.234");

            Assert.That(model.Id, Is.EqualTo(100));
            Assert.That(model.Name, Is.EqualTo("Ampla"));
            Assert.That(model.Value, Is.EqualTo(1.234D));
        }

        [Test]
        public void TrySetPropertyWithEmptyValues()
        {
            SimpleModel model = new SimpleModel { Id = 1, Name = "Name", Value = 1.234};

            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            AssertPropertySetValue(modelProperties, model, "Id", "0");
            AssertPropertySetValue(modelProperties, model, "Name", "");
            AssertPropertySetValue(modelProperties, model, "Value", "0");

            Assert.That(model.Id, Is.EqualTo(0));
            Assert.That(model.Name, Is.EqualTo(""));
            Assert.That(model.Value, Is.EqualTo(0));
        }

        [Test]
        public void TrySetPropertyWithNullValues()
        {
            SimpleModel model = new SimpleModel { Id = 1, Name = "Name", Value = 1.234 };

            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            AssertPropertyNotSetValue(modelProperties, model, "Id", null);
            AssertPropertySetValue(modelProperties, model, "Name", null);
            AssertPropertyNotSetValue(modelProperties, model, "Value", null);

            Assert.That(model.Id, Is.EqualTo(1));
            Assert.That(model.Name, Is.EqualTo(null));
            Assert.That(model.Value, Is.EqualTo(1.234));
        }

        [Test]
        public void RoundTripProperties()
        {
            SimpleModel model = new SimpleModel { Id = 100, Name = "Ampla", Value = 1.234 }; 
            SimpleModel newModel = new SimpleModel();

            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            string id;
            string name;
            string value;

            modelProperties.TryGetPropertyValue(model, "Id", out id);
            modelProperties.TryGetPropertyValue(model, "Name", out name);
            modelProperties.TryGetPropertyValue(model, "Value", out value);

            modelProperties.TrySetValueFromString(newModel, "Id", id);
            modelProperties.TrySetValueFromString(newModel, "Name", name);
            modelProperties.TrySetValueFromString(newModel, "Value", value);

            Assert.That(newModel.Id, Is.EqualTo(100));
            Assert.That(newModel.Name, Is.EqualTo("Ampla"));
            Assert.That(newModel.Value, Is.EqualTo(1.234D));
        }

        [Test]
        public void RoundTripNullProperties()
        {
            SimpleModel model = new SimpleModel { Id = 1, Name = null, Value = 0 };
            SimpleModel newModel = new SimpleModel();

            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            string id;
            string name;
            string value;

            modelProperties.TryGetPropertyValue(model, "Id", out id);
            modelProperties.TryGetPropertyValue(model, "Name", out name);
            modelProperties.TryGetPropertyValue(model, "Value", out value);

            modelProperties.TrySetValueFromString(newModel, "Id", id);
            modelProperties.TrySetValueFromString(newModel, "Name", name);
            modelProperties.TrySetValueFromString(newModel, "Value", value);

            Assert.That(newModel.Id, Is.EqualTo(1));
            Assert.That(newModel.Name, Is.EqualTo(null));
            Assert.That(newModel.Value, Is.EqualTo(0));
        }

        [Test]
        public void RoundTripEmptyProperties()
        {
            SimpleModel model = new SimpleModel { Id = 1, Name = string.Empty, Value = 0 };
            SimpleModel newModel = new SimpleModel();

            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            string id;
            string name;
            string value;

            modelProperties.TryGetPropertyValue(model, "Id", out id);
            modelProperties.TryGetPropertyValue(model, "Name", out name);
            modelProperties.TryGetPropertyValue(model, "Value", out value);

            modelProperties.TrySetValueFromString(newModel, "Id", id);
            modelProperties.TrySetValueFromString(newModel, "Name", name);
            modelProperties.TrySetValueFromString(newModel, "Value", value);

            Assert.That(newModel.Id, Is.EqualTo(1));
            Assert.That(newModel.Name, Is.EqualTo(string.Empty));
            Assert.That(newModel.Value, Is.EqualTo(0));
        }

        [Test]
        public void TestWriteOnlyProperties()
        {
            ModelWithWriteOnly model = new ModelWithWriteOnly();

            CalendarProperties<ModelWithWriteOnly> modelProperties = new CalendarProperties<ModelWithWriteOnly>();

            Assert.That(modelProperties.GetProperties().Count, Is.EqualTo(4));

            AssertPropertySetValue(modelProperties, model, "Id", "100");
            AssertPropertySetValue(modelProperties, model, "Name", "old name");
            AssertPropertySetValue(modelProperties, model, "Value", "123.4");

            Assert.That(model.Id, Is.EqualTo(100));
            Assert.That(model.Name, Is.EqualTo("old name"));
            Assert.That(model.Value, Is.EqualTo(123.4));

            AssertPropertySetValue(modelProperties, model, "WriteOnlyName", "new name");
            Assert.That(model.Name, Is.EqualTo("new name"));

            AssertPropertyNotGetValue(modelProperties, model, "WriteOnlyName");
        }

        [Test]
        public void TestReadOnlyProperties()
        {
            ModelWithReadOnly model = new ModelWithReadOnly();

            CalendarProperties<ModelWithReadOnly> modelProperties = new CalendarProperties<ModelWithReadOnly>();

            Assert.That(modelProperties.GetProperties().Count, Is.EqualTo(4));

            model.Name = "name1";
            AssertPropertyGetValue(modelProperties, model, "ReadOnlyName", "name1");
            AssertPropertyNotSetValue(modelProperties, model, "ReadOnlyName", "invalid name");
        }

        [Test]
        public void IsDefaultValue()
        {
            SimpleModel model = new SimpleModel();
            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            Assert.That(modelProperties.IsDefaultValue(model, "Id"), Is.True, "Id");
            Assert.That(modelProperties.IsDefaultValue(model, "Value"), Is.True, "Value");
            model.Id = 100;
            Assert.That(modelProperties.IsDefaultValue(model, "Id"), Is.False, "Id");
            model.Value = 12.34;
            Assert.That(modelProperties.IsDefaultValue(model, "Value"), Is.False, "Value");
        }

        [Test]
        public void CloneModel()
        {
            SimpleModel model = new SimpleModel();

            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            SimpleModel clone = modelProperties.CloneModel(model);

            Assert.That(ReferenceEquals(model, clone), Is.False);

            AssertPropertyGetValue(modelProperties, model, "Id", "0");
            AssertPropertyGetValue(modelProperties, model, "Name", null);
            AssertPropertyGetValue(modelProperties, model, "Value", "0");

            AssertPropertyGetValue(modelProperties, clone, "Id", "0");
            AssertPropertyGetValue(modelProperties, clone, "Name", null);
            AssertPropertyGetValue(modelProperties, clone, "Value", "0");

            model.Id++;

            Assert.That(clone.Id, Is.Not.EqualTo(model.Id));
        }


        [Test]
        public void CloneModelWithValues()
        {
            SimpleModel model = new SimpleModel { Id = 100, Name = "Ampla", Value = 1.234 }; 

            CalendarProperties<SimpleModel> modelProperties = new CalendarProperties<SimpleModel>();

            SimpleModel clone = modelProperties.CloneModel(model);

            Assert.That(ReferenceEquals(model, clone), Is.False);

            AssertPropertyGetValue(modelProperties, model, "Id", "100");
            AssertPropertyGetValue(modelProperties, model, "Name", "Ampla");
            AssertPropertyGetValue(modelProperties, model, "Value", "1.234");

            AssertPropertyGetValue(modelProperties, clone, "Id", "100");
            AssertPropertyGetValue(modelProperties, clone, "Name", "Ampla");
            AssertPropertyGetValue(modelProperties, clone, "Value", "1.234");

            model.Id++;

            Assert.That(clone.Id, Is.Not.EqualTo(model.Id));
        }

        private void AssertPropertyGetValue<TModel>(CalendarProperties<TModel> modelProperties, TModel model, string property, string expected) where TModel : new()
        {
            string value;
            bool result = modelProperties.TryGetPropertyValue(model, property, out value);

            Assert.That(result, Is.True, "Unexpected Result for {0}", property);
            Assert.That(value, Is.EqualTo(expected), "TryGetPropertyValue('{0}')", property);
        }

        private void AssertPropertySetValue<TModel>(CalendarProperties<TModel> modelProperties, TModel model, string property, string value) where TModel : new()
        {
            bool result = modelProperties.TrySetValueFromString(model, property, value);
            Assert.That(result, Is.True, "Unexpected Result for {0}", property);
        }

        private void AssertPropertyNotSetValue<TModel>(CalendarProperties<TModel> modelProperties, TModel model, string property, string value) where TModel : new()
        {
            bool result = modelProperties.TrySetValueFromString(model, property, value);
            Assert.That(result, Is.False, "Unexpected Result for {0}", property);
        }

        private void AssertPropertyNotGetValue<TModel>(CalendarProperties<TModel> modelProperties, TModel model, string property) where TModel : new()
        {
            string value;
            bool result = modelProperties.TryGetPropertyValue(model, property, out value);

            Assert.That(result, Is.False, "Unexpected Result for {0}", property);
        }

    }
}