namespace AmplaWeb.Data.Binding.ViewData
{
    public interface IViewPermissions
    {
        bool CanView();
        bool CanAdd();
        bool CanDelete();
        bool CanModify();
        bool CanConfirm();
        bool CanUnconfirm();
        bool CanSplit();
    }
}