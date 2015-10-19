using System;

namespace AmplaData.Binding.ViewData
{
    public abstract class ViewPermissionsAdapter : IViewPermissions
    {
        private readonly string module;
        private readonly IViewPermissions viewPermissions;
        private readonly IViewPermissions modulePermissions;

        protected ViewPermissionsAdapter(string module, IViewPermissions viewPermissions,
                                             IViewPermissions modulePermissions)
        {
            if (viewPermissions == null) throw new ArgumentNullException("viewPermissions");
            this.module = module;
            this.viewPermissions = viewPermissions;
            this.modulePermissions = modulePermissions;
        }

        protected abstract void InvalidPermissionAction(string message);

        protected IViewPermissions ModulePermissions { get { return modulePermissions; } }

        public bool CanView()
        {
            bool permission = viewPermissions.CanView();
            ViewPermissionNotGranted(permission, "ViewRecord");

            bool modulePermission = modulePermissions.CanView();
            ViewPermissionNotGranted(modulePermission, "ViewRecord");

            return permission && modulePermission;
        }

        public bool CanAdd()
        {
            const string operation = "AddRecord";
            bool permission = viewPermissions.CanAdd();
            ViewPermissionNotGranted(permission, operation);

            bool modulePermission = modulePermissions.CanAdd();
            ModuleNotValid(modulePermission, operation);

            return permission && modulePermission;
        }

        public bool CanDelete()
        {
            const string operation = "DeleteRecord";
            bool permission = viewPermissions.CanDelete();
            ViewPermissionNotGranted(permission, operation);

            bool modulePermission = modulePermissions.CanDelete();
            ModuleNotValid(modulePermission, operation);

            return permission && modulePermission;
        }

        public bool CanModify()
        {
            const string operation = "ModifyRecord";
            bool permission = viewPermissions.CanModify();
            ViewPermissionNotGranted(permission, operation);

            bool modulePermission = modulePermissions.CanModify();
            ModuleNotValid(modulePermission, operation);

            return permission && modulePermission;
        }

        public bool CanConfirm()
        {
            const string operation = "ConfirmRecord";
            bool permission = viewPermissions.CanConfirm();
            ViewPermissionNotGranted(permission, operation);

            bool modulePermission = modulePermissions.CanConfirm();
            ModuleNotValid(modulePermission, operation);

            return permission && modulePermission;
        }

        public bool CanUnconfirm()
        {
            const string operation = "UnconfirmRecord";
            bool permission = viewPermissions.CanUnconfirm();
            ViewPermissionNotGranted(permission, operation);

            bool modulePermission = modulePermissions.CanUnconfirm();
            ModuleNotValid(modulePermission, operation);

            return permission && modulePermission;
        }

        public bool CanSplit()
        {
            const string operation = "SplitRecord";
            bool permission = viewPermissions.CanSplit();
            ViewPermissionNotGranted(permission, operation);

            bool modulePermission = modulePermissions.CanSplit();
            ModuleNotValid(modulePermission, operation);

            return permission && modulePermission;
        }

        private void ViewPermissionNotGranted(bool permission, string operation)
        {
            if (permission) return;
            string message = "Permission '" + operation + "' is not granted for view.";

            InvalidPermissionAction(message);
        }

        private void ModuleNotValid(bool permission, string operation)
        {
            if (permission) return;

            string message = string.Format("Permission '{0}' is not valid for '{1}'.", operation, module);
            InvalidPermissionAction(message);
        }

    }
}