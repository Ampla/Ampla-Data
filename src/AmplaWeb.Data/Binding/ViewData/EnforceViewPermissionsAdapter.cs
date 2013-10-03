using System;

namespace AmplaWeb.Data.Binding.ViewData
{
    public class EnforceViewPermissionsAdapter : IViewPermissions
    {
        private readonly IViewPermissions viewPermissions;

        public EnforceViewPermissionsAdapter(IViewPermissions viewPermissions)
        {
            if (viewPermissions == null) throw new ArgumentNullException("viewPermissions");
            this.viewPermissions = viewPermissions;
        }

        public bool CanView()
        {
            bool permission = viewPermissions.CanView();
            ThrowIfNotGranted(permission, "ViewRecord");
            return permission;
        }

        public bool CanAdd()
        {
            bool permission = viewPermissions.CanAdd();
            ThrowIfNotGranted(permission, "AddRecord");
            return permission;
        }

        public bool CanDelete()
        {
            bool permission = viewPermissions.CanDelete();
            ThrowIfNotGranted(permission, "DeleteRecord");
            return permission;
        }

        public bool CanModify()
        {
            bool permission = viewPermissions.CanModify();
            ThrowIfNotGranted(permission, "ModifyRecord");
            return permission;
        }

        public bool CanConfirm()
        {
            bool permission = viewPermissions.CanConfirm();
            ThrowIfNotGranted(permission, "ConfirmRecord");
            return permission;
        }

        public bool CanUnconfirm()
        {
            bool permission = viewPermissions.CanUnconfirm();
            ThrowIfNotGranted(permission, "UnconfirmRecord");
            return permission;
        }

        public bool CanSplit()
        {
            bool permission = viewPermissions.CanSplit();
            ThrowIfNotGranted(permission, "SplitRecord");
            return permission;
        }

        private static void ThrowIfNotGranted(bool permission, string operation)
        {
            if (!permission)
            {
                throw new InvalidOperationException("Permission '" + operation + "' is not granted.");
            }
        }
    }
}