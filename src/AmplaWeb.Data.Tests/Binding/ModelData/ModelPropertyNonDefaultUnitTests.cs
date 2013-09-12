using System;
using System.ComponentModel;
using AmplaWeb.Data.Attributes;
using AmplaWeb.Data.Tests;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.ModelData
{
    [TestFixture]
    public class ModelPropertyNonDefaultUnitTests : TestFixture
    {
        public class ModelWithAmplaField
        {   
            [AmplaField("Full Name")]
            public string FullName
            {
                get; set;
            }
        }

        [Test]
        public void AmplaFieldSpecified()
        {
            
        }
    }
}