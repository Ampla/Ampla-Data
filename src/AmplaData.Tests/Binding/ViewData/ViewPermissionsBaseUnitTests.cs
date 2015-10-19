using System;
using AmplaData.AmplaData2008;
using NUnit.Framework;

namespace AmplaData.Binding.ViewData
{
    [TestFixture]
    public abstract class ViewPermissionsBaseUnitTests : TestFixture
    {
        private ViewPermissions permissions;
        private IViewPermissions viewPermissions;

        protected abstract IViewPermissions CreateViewPermissions(ViewPermissions permissions);

        protected abstract void AssertTrue(Func<bool> assert, string permission);

        protected abstract void AssertFalse(Func<bool> assert, string permission);

        protected override void OnSetUp()
        {
            base.OnSetUp();
            Create(() => new ViewPermissions());
        }

        private void Create(Func<ViewPermissions> createFunc)
        {
            permissions = createFunc();
            viewPermissions = CreateViewPermissions(permissions);
        }

        [Test]
        public void Default()
        {
            AssertAllFalse();
        }

        [Test]
        public void ConstructorNull()
        {
            Create(() => new ViewPermissions(null));
            AssertAllFalse();
        }

        [Test]
        public void ConstructorCanAdd()
        {
            Create(() => new ViewPermissions(ViewAllowedOperations.AddRecord));
            AssertAllFalseExcept("Add");
        }

        [Test]
        public void ConstructorCanConfirm()
        {
            Create(() => new ViewPermissions(ViewAllowedOperations.ConfirmRecord));
            AssertAllFalseExcept("Confirm");
        }

        [Test]
        public void ConstructorCanDelete()
        {
            Create(() => new ViewPermissions(ViewAllowedOperations.DeleteRecord));
            AssertAllFalseExcept("Delete");
        }

        [Test]
        public void ConstructorCanModify()
        {
            Create(() => new ViewPermissions(ViewAllowedOperations.ModifyRecord));
            AssertAllFalseExcept("Modify");
        }

        [Test]
        public void ConstructorCanSplit()
        {
            Create(() => new ViewPermissions(ViewAllowedOperations.SplitRecord));
            AssertAllFalseExcept("Split");
        }

        [Test]
        public void ConstructorCanUnconfirm()
        {
            Create(() => new ViewPermissions(ViewAllowedOperations.UnconfirmRecord));
            AssertAllFalseExcept("Unconfirm");
        }

        [Test]
        public void ConstructorCanView()
        {
            Create(() => new ViewPermissions(ViewAllowedOperations.ViewRecord));
            AssertAllFalseExcept("View");
        }

        [Test]
        public void InitialiseNull()
        {
            permissions.Initialise(null);
            AssertAllFalse();
        }

        [Test]
        public void InitialiseCanAdd()
        {
            permissions.Initialise(new[] { Add() });
            AssertAllFalseExcept("Add");
        }

        [Test]
        public void InitialiseCanConfirm()
        {
            permissions.Initialise(new[] { Confirm() });
            AssertAllFalseExcept("Confirm");
        }

        [Test]
        public void InitialiseCanDelete()
        {
            permissions.Initialise(new[] { Delete() });
            AssertAllFalseExcept("Delete");
        }

        [Test]
        public void InitialiseCanModify()
        {
            permissions.Initialise(new[] { Modify() });
            AssertAllFalseExcept("Modify");
        }

        [Test]
        public void InitialiseCanSplit()
        {
            permissions.Initialise(new[] { Split() });
            AssertAllFalseExcept("Split");
        }

        [Test]
        public void InitialiseCanUnconfirm()
        {
            permissions.Initialise(new[] { Unconfirm() });
            AssertAllFalseExcept("Unconfirm");
        }

        [Test]
        public void InitialiseCanView()
        {
            permissions.Initialise(new[] { View() });
            AssertAllFalseExcept("View");
        }

        private void AssertAllFalse()
        {
            AssertMethod(viewPermissions.CanAdd, false, "Add");
            AssertMethod(viewPermissions.CanConfirm, false, "Confirm");
            AssertMethod(viewPermissions.CanDelete, false, "Delete");
            AssertMethod(viewPermissions.CanModify, false, "Modify");
            AssertMethod(viewPermissions.CanSplit, false, "Split");
            AssertMethod(viewPermissions.CanUnconfirm, false, "Unconfirm");
            AssertMethod(viewPermissions.CanView, false, "View");
        }

        private void AssertMethod(Func<bool> func, bool result, string operation)
        {
            if (result)
            {
                AssertTrue(func, operation);
            }
            else
            {
                AssertFalse(func, operation);
            }
        }

        private void AssertAllFalseExcept(string operation)
        {
            AssertMethod(viewPermissions.CanAdd, operation == "Add", "Add");
            AssertMethod(viewPermissions.CanConfirm, operation == "Confirm", "Confirm");
            AssertMethod(viewPermissions.CanDelete, operation == "Delete", "Delete");
            AssertMethod(viewPermissions.CanModify, operation == "Modify", "Modify");
            AssertMethod(viewPermissions.CanSplit, operation == "Split", "Split");
            AssertMethod(viewPermissions.CanUnconfirm, operation == "Unconfirm", "Unconfirm");
            AssertMethod(viewPermissions.CanView, operation == "View", "View");
        }

        protected static GetViewsAllowedOperation Add()
        {
            return new GetViewsAllowedOperation { Operation = ViewAllowedOperations.AddRecord, Allowed = true };
        }

        protected static GetViewsAllowedOperation Confirm()
        {
            return new GetViewsAllowedOperation { Operation = ViewAllowedOperations.ConfirmRecord, Allowed = true };
        }

        protected static GetViewsAllowedOperation Delete()
        {
            return new GetViewsAllowedOperation { Operation = ViewAllowedOperations.DeleteRecord, Allowed = true };
        }

        protected static GetViewsAllowedOperation Modify()
        {
            return new GetViewsAllowedOperation { Operation = ViewAllowedOperations.ModifyRecord, Allowed = true };
        }

        protected static GetViewsAllowedOperation Split()
        {
            return new GetViewsAllowedOperation { Operation = ViewAllowedOperations.SplitRecord, Allowed = true };
        }

        private static GetViewsAllowedOperation Unconfirm()
        {
            return new GetViewsAllowedOperation { Operation = ViewAllowedOperations.UnconfirmRecord, Allowed = true };
        }

        protected static GetViewsAllowedOperation View()
        {
            return new GetViewsAllowedOperation { Operation = ViewAllowedOperations.ViewRecord, Allowed = true };
        }
    }
}