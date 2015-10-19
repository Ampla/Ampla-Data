using System;
using AmplaData.AmplaData2008;
using NUnit.Framework;

namespace AmplaData.Binding.ViewData
{
    [TestFixture]
    public class ViewPermissionsUnitTests : ViewPermissionsBaseUnitTests
    {
        protected override IViewPermissions CreateViewPermissions(ViewPermissions permissions)
        {
            return permissions;
        }

        protected override void AssertTrue(Func<bool> assert, string operation)
        {
            Assert.That(assert(), Is.True, "Operation: {0}", operation);
        }

        protected override void AssertFalse(Func<bool> assert, string operation)
        {
            Assert.That(assert(), Is.False, "Operation: {0}", operation);
        }
    }
}