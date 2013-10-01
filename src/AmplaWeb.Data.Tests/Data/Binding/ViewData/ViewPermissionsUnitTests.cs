using AmplaWeb.Data.AmplaData2008;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.ViewData
{
    [TestFixture]
    public class ViewPermissionsUnitTests : TestFixture
    {
        [Test]
        public void Default()
        {
            ViewPermissions permissions = new ViewPermissions();
            AssertAllFalse(permissions);
        }
        
        [Test]
        public void InitialiseNull()
        {
            ViewPermissions permissions = new ViewPermissions();
            permissions.Initialise(null);
            AssertAllFalse(permissions);
        }
        
        [Test]
        public void InitialiseCanAdd()
        {
            ViewPermissions permissions = new ViewPermissions();
            permissions.Initialise(new [] { Add()});
            AssertAllFalseExcept(permissions, "Add");
        }

        [Test]
        public void InitialiseCanConfirm()
        {
            ViewPermissions permissions = new ViewPermissions();
            permissions.Initialise(new[] { Confirm() });
            AssertAllFalseExcept(permissions, "Confirm");
        }

        [Test]
        public void InitialiseCanDelete()
        {
            ViewPermissions permissions = new ViewPermissions();
            permissions.Initialise(new[] { Delete() });
            AssertAllFalseExcept(permissions, "Delete");
        }

        [Test]
        public void InitialiseCanModify()
        {
            ViewPermissions permissions = new ViewPermissions();
            permissions.Initialise(new[] { Modify() });
            AssertAllFalseExcept(permissions, "Modify");
        }

        [Test]
        public void InitialiseCanSplit()
        {
            ViewPermissions permissions = new ViewPermissions();
            permissions.Initialise(new[] { Split() });
            AssertAllFalseExcept(permissions, "Split");
        }

        [Test]
        public void InitialiseCanUnconfirm()
        {
            ViewPermissions permissions = new ViewPermissions();
            permissions.Initialise(new[] { Unconfirm() });
            AssertAllFalseExcept(permissions, "Unconfirm");
        }

        [Test]
        public void InitialiseCanView()
        {
            ViewPermissions permissions = new ViewPermissions();
            permissions.Initialise(new[] { View() });
            AssertAllFalseExcept(permissions, "View");
        }


        private static void AssertAllFalse(ViewPermissions permissions)
        {
            Assert.That(permissions.CanAdd, Is.False);
            Assert.That(permissions.CanConfirm, Is.False);
            Assert.That(permissions.CanDelete, Is.False);
            Assert.That(permissions.CanModify, Is.False);
            Assert.That(permissions.CanSplit, Is.False);
            Assert.That(permissions.CanUnconfirm, Is.False);
            Assert.That(permissions.CanView, Is.False);
        }

        private static void AssertAllFalseExcept(ViewPermissions permissions, string operation)
        {
            Assert.That(permissions.CanAdd, Is.EqualTo(operation == "Add"));
            Assert.That(permissions.CanConfirm, Is.EqualTo(operation == "Confirm"));
            Assert.That(permissions.CanDelete, Is.EqualTo(operation == "Delete"));
            Assert.That(permissions.CanModify, Is.EqualTo(operation == "Modify"));
            Assert.That(permissions.CanSplit, Is.EqualTo(operation == "Split"));
            Assert.That(permissions.CanUnconfirm, Is.EqualTo(operation == "Unconfirm"));
            Assert.That(permissions.CanView, Is.EqualTo(operation == "View"));
        }

        private static GetViewsAllowedOperation Add()
        {
            return new GetViewsAllowedOperation {Operation = ViewAllowedOperations.AddRecord, Allowed = true};
        }

        private static GetViewsAllowedOperation Confirm()
        {
            return new GetViewsAllowedOperation { Operation = ViewAllowedOperations.ConfirmRecord, Allowed = true };
        }

        private static GetViewsAllowedOperation Delete()
        {
            return new GetViewsAllowedOperation { Operation = ViewAllowedOperations.DeleteRecord, Allowed = true };
        }

        private static GetViewsAllowedOperation Modify()
        {
            return new GetViewsAllowedOperation { Operation = ViewAllowedOperations.ModifyRecord, Allowed = true };
        }

        private static GetViewsAllowedOperation Split()
        {
            return new GetViewsAllowedOperation { Operation = ViewAllowedOperations.SplitRecord, Allowed = true };
        }

        private static GetViewsAllowedOperation Unconfirm()
        {
            return new GetViewsAllowedOperation { Operation = ViewAllowedOperations.UnconfirmRecord, Allowed = true };
        }

        private static GetViewsAllowedOperation View()
        {
            return new GetViewsAllowedOperation { Operation = ViewAllowedOperations.ViewRecord, Allowed = true };
        }
    }
}