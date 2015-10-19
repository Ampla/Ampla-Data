using System.Collections.Generic;

namespace AmplaData.Binding.ViewData
{
    public class ValidateViewPermissions : ViewPermissionsAdapter
    {
        public ValidateViewPermissions(string module, IViewPermissions viewPermissions, IViewPermissions modulePermissions) : base(module, viewPermissions, modulePermissions)
        {
            Messages = new List<string>();
        }

        protected override void InvalidPermissionAction(string message)
        {
            Messages.Add(message);
        }

        public List<string> Messages { get; private set; }

        public void ValidatePermissions()
        {
            if (ModulePermissions.CanAdd())
            {
                CanAdd();
            }
            if (ModulePermissions.CanConfirm())
            {
                CanConfirm();
            }
            if (ModulePermissions.CanDelete())
            {
                CanDelete();
            }
            if (ModulePermissions.CanModify())
            {
                CanModify();
            }
            if (ModulePermissions.CanSplit())
            {
                CanSplit();
            }
            if (ModulePermissions.CanUnconfirm())
            {
                CanUnconfirm();
            }
            if (ModulePermissions.CanView())
            {
                CanView();
            }
        }
    }
}