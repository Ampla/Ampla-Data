using System;

namespace AmplaData.Binding.ViewData
{
    public class EnforceViewPermissionsAdapter : ViewPermissionsAdapter
    {
        public EnforceViewPermissionsAdapter(string module, IViewPermissions viewPermissions, IViewPermissions modulePermissions) : base(module, viewPermissions, modulePermissions)
        {
        }

        protected override void InvalidPermissionAction(string message)
        {
            throw new InvalidOperationException(message);
        }
    }
}