using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Binding.ViewData
{
    /// <summary>
    ///     Represents whether the View can support the record operations
    /// </summary>
    public class ViewPermissions : IViewPermissions
    {
        public ViewPermissions()
        {
            canAdd = false;
            canDelete = false;
            canView = false;
            canConfirm = false;
            canModify = false;
            canSplit = false;
            canUnconfirm = false;
        }

        private bool canView;

        public bool CanView()
        {
            return canView;
        }

        private bool canAdd;

        public bool CanAdd()
        {
            return canAdd;
        }

        private bool canDelete;

        public bool CanDelete()
        {
            return canDelete;
        }

        private bool canModify;

        public bool CanModify()
        {
            return canModify;
        }

        private bool canConfirm;

        public bool CanConfirm()
        {
            return canConfirm;
        }

        private bool canUnconfirm;

        public bool CanUnconfirm()
        {
            return canUnconfirm;
        }

        private bool canSplit;

        public bool CanSplit()
        {
            return canSplit;
        }

        /// <summary>
        ///     Initialise the record permissions from the GetViews 
        /// </summary>
        /// <param name="allowedOperations"></param>
        public void Initialise(GetViewsAllowedOperation[] allowedOperations)
        {
            if (allowedOperations != null)
            {
                foreach (GetViewsAllowedOperation operation in allowedOperations)
                {
                    switch (operation.Operation)
                    {
                        case ViewAllowedOperations.AddRecord:
                            {
                                canAdd = operation.Allowed;
                                break;
                            }
                        case ViewAllowedOperations.ConfirmRecord:
                            {
                                canConfirm = operation.Allowed;
                                break;
                            }
                        case ViewAllowedOperations.DeleteRecord:
                            {
                                canDelete = operation.Allowed;
                                break;
                            }
                        case ViewAllowedOperations.ModifyRecord:
                            {
                                canModify = operation.Allowed;
                                break;
                            }

                        case ViewAllowedOperations.SplitRecord:
                            {
                                canSplit = operation.Allowed;
                                break;
                            }
                        case ViewAllowedOperations.UnconfirmRecord:
                            {
                                canUnconfirm = operation.Allowed;
                                break;
                            }
                        case ViewAllowedOperations.ViewRecord:
                            {
                                canView = operation.Allowed;
                                break;
                            }
                    }
                }
            }
        }
    }
}