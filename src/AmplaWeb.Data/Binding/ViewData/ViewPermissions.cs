using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Binding.ViewData
{
    /// <summary>
    ///     Represents whether the View can support the record operations
    /// </summary>
    public class ViewPermissions
    {
        public ViewPermissions()
        {
            CanAdd = false;
            CanDelete = false;
            CanView = false;
            CanConfirm = false;
            CanModify = false;
            CanSplit = false;
            CanUnconfirm = false;
        }

        public bool CanView { get; private set; }

        public bool CanAdd { get; private set; }
        
        public bool CanDelete { get; private set; }
        
        public bool CanModify { get; private set; }

        public bool CanConfirm { get; private set; }
        
        public bool CanUnconfirm { get; private set; }

        public bool CanSplit { get; private set; }

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
                                CanAdd = operation.Allowed;
                                break;
                            }
                        case ViewAllowedOperations.ConfirmRecord:
                            {
                                CanConfirm = operation.Allowed;
                                break;
                            }
                        case ViewAllowedOperations.DeleteRecord:
                            {
                                CanDelete = operation.Allowed;
                                break;
                            }
                        case ViewAllowedOperations.ModifyRecord:
                            {
                                CanModify = operation.Allowed;
                                break;
                            }

                        case ViewAllowedOperations.SplitRecord:
                            {
                                CanSplit = operation.Allowed;
                                break;
                            }
                        case ViewAllowedOperations.UnconfirmRecord:
                            {
                                CanUnconfirm = operation.Allowed;
                                break;
                            }
                        case ViewAllowedOperations.ViewRecord:
                            {
                                CanView = operation.Allowed;
                                break;
                            }
                    }
                }
            }
        }
    }
}