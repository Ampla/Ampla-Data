using System;
using AmplaData.AmplaData2008;
using NUnit.Framework;

namespace AmplaData.Binding.ViewData
{
    [TestFixture]
    public class ValidateViewPermissionsUnitTests : ViewPermissionsBaseUnitTests
    {
        private ValidateViewPermissions viewPermissions;
        private readonly ViewPermissions downtimePermissions = new ViewPermissions(
                ViewAllowedOperations.AddRecord,
                ViewAllowedOperations.ConfirmRecord,
                ViewAllowedOperations.DeleteRecord,
                ViewAllowedOperations.ModifyRecord,
                ViewAllowedOperations.SplitRecord,
                ViewAllowedOperations.UnconfirmRecord,
                ViewAllowedOperations.ViewRecord);

        private readonly ViewPermissions productionPermissions = new ViewPermissions(
        ViewAllowedOperations.AddRecord,
        ViewAllowedOperations.ConfirmRecord,
        ViewAllowedOperations.DeleteRecord,
        ViewAllowedOperations.ModifyRecord,
        ViewAllowedOperations.UnconfirmRecord,
        ViewAllowedOperations.ViewRecord);

        protected override IViewPermissions CreateViewPermissions(ViewPermissions permissions)
        {
            viewPermissions = new ValidateViewPermissions("Downtime", permissions, downtimePermissions);
            return viewPermissions;
        }

        protected override void AssertTrue(Func<bool> assert, string operation)
        {
            int existingMessages = viewPermissions.Messages.Count;
            bool result = assert();

            Assert.That(result, Is.True);
            Assert.That(viewPermissions.Messages.Count, Is.EqualTo(existingMessages));
        }

        protected override void AssertFalse(Func<bool> assert, string permission)
        {
            int existingMessages = viewPermissions.Messages.Count;

            bool result = assert();
            Assert.That(result, Is.False);

            Assert.That(viewPermissions.Messages.Count, Is.GreaterThan(existingMessages));
            string message = viewPermissions.Messages[viewPermissions.Messages.Count - 1];
            Assert.That(message, Is.StringContaining(permission));
        }

        [Test]
        public void ValidatesNoPermissions()
        {
            ViewPermissions permissions = new ViewPermissions();
            ValidateViewPermissions view = new ValidateViewPermissions("Production", permissions, productionPermissions);

            Assert.That(view.Messages, Is.Empty);
            view.ValidatePermissions();
            Assert.That(view.Messages, Is.Not.Empty);
            Assert.That(view.Messages.Count, Is.EqualTo(6));
        }

        [Test]
        public void ValidatesViewOnly()
        {
            ViewPermissions permissions = new ViewPermissions();
            permissions.Initialise(new [] {View()});
            ValidateViewPermissions view = new ValidateViewPermissions("Production", permissions, productionPermissions);

            Assert.That(view.Messages, Is.Empty);
            view.ValidatePermissions();
            Assert.That(view.Messages, Is.Not.Empty);
            Assert.That(view.Messages.Count, Is.EqualTo(5));
        }

        [Test]
        public void ValidatesViewAddModifyOnly()
        {
            ViewPermissions permissions = new ViewPermissions();
            permissions.Initialise(new[] { View(), Add(), Modify() });
            ValidateViewPermissions view = new ValidateViewPermissions("Production", permissions, productionPermissions);

            Assert.That(view.Messages, Is.Empty);
            view.ValidatePermissions();
            Assert.That(view.Messages, Is.Not.Empty);
            Assert.That(view.Messages.Count, Is.EqualTo(3));
        }
    }
}