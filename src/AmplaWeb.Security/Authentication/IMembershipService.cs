

namespace AmplaWeb.Security.Authentication
{
    public interface IMembershipService
    {
        bool ValidateUser(string userName, string password);
    }
}