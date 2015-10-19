using System;
using AmplaData.AmplaData2008;
using NUnit.Framework;

namespace AmplaData.Binding.ViewData
{
    [TestFixture]
    public class EnsureViewPermissionsAdapterUnitTests : ViewPermissionsBaseUnitTests
    {
        protected override IViewPermissions CreateViewPermissions(ViewPermissions permissions)
        {
            ViewPermissions modulePermissions = new ViewPermissions(
                ViewAllowedOperations.AddRecord,
                ViewAllowedOperations.ConfirmRecord,
                ViewAllowedOperations.DeleteRecord,
                ViewAllowedOperations.ModifyRecord,
                ViewAllowedOperations.SplitRecord,
                ViewAllowedOperations.UnconfirmRecord,
                ViewAllowedOperations.ViewRecord);
            return new EnforceViewPermissionsAdapter("Downtime", permissions, modulePermissions);
        }

        protected override void AssertTrue(Func<bool> assert, string operation)
        {
            Assert.That(assert(), Is.True, "Operation: {0}", operation);
        }

        protected override void AssertFalse(Func<bool> assert, string operation)
        {
            Assert.Throws<InvalidOperationException>(() => assert(), "Operation: {0}", operation);
        }

        [Test]
        public void NullConstructor()
        {
            Assert.Throws<ArgumentNullException>(() => new EnforceViewPermissionsAdapter("module", null, new ViewPermissions()));
        }
    }
}