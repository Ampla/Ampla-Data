using System;
using AmplaData.Data.AmplaData2008;
using NUnit.Framework;

namespace AmplaData.Data.Binding.ViewData
{
    [TestFixture]
    public class ViewPermissionsUnitTests : ViewPermissionsBaseUnitTests
    {
        protected override IViewPermissions CreateViewPermissions(ViewPermissions permissions)
        {
            return permissions;
        }

        protected override void AssertTrue(Func<bool> assert)
        {
            Assert.That(assert(), Is.True);
        }

        protected override void AssertFalse(Func<bool> assert)
        {
            Assert.That(assert(), Is.False);
        }

    }

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

        protected override void AssertTrue(Func<bool> assert)
        {
            Assert.That(assert(), Is.True);
        }

        protected override void AssertFalse(Func<bool> assert)
        {
            Assert.Throws<InvalidOperationException>(()=>assert());
        }

        [Test]
        public void NullConstructor()
        {
            Assert.Throws<ArgumentNullException>(() => new EnforceViewPermissionsAdapter("module", null, new ViewPermissions()));
        }
    }

    [TestFixture]
    public abstract class ViewPermissionsBaseUnitTests : TestFixture
    {
        private ViewPermissions permissions;
        private IViewPermissions viewPermissions;

        protected abstract IViewPermissions CreateViewPermissions(ViewPermissions permissions);

        protected abstract void AssertTrue(Func<bool> assert);

        protected abstract void AssertFalse(Func<bool> assert);

        protected override void OnSetUp()
        {
            base.OnSetUp();
            Create(() => new ViewPermissions());
        }

        private void Create(Func<ViewPermissions> createFunc )
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
            AssertMethod(viewPermissions.CanAdd, false);
            AssertMethod(viewPermissions.CanConfirm, false);
            AssertMethod(viewPermissions.CanDelete, false);
            AssertMethod(viewPermissions.CanModify, false);
            AssertMethod(viewPermissions.CanSplit, false);
            AssertMethod(viewPermissions.CanUnconfirm, false);
            AssertMethod(viewPermissions.CanView, false);
        }

        private void AssertMethod(Func<bool> func, bool result)
        {
            if (result)
            {
                AssertTrue(func);
            }
            else
            {
                AssertFalse(func);
            }
        }
        
        private void AssertAllFalseExcept(string operation)
        {
            AssertMethod(viewPermissions.CanAdd, operation == "Add");
            AssertMethod(viewPermissions.CanConfirm, operation == "Confirm");
            AssertMethod(viewPermissions.CanDelete, operation == "Delete");
            AssertMethod(viewPermissions.CanModify, operation == "Modify");
            AssertMethod(viewPermissions.CanSplit, operation == "Split");
            AssertMethod(viewPermissions.CanUnconfirm, operation == "Unconfirm");
            AssertMethod(viewPermissions.CanView, operation == "View");
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