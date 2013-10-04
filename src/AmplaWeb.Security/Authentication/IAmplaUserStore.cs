namespace AmplaWeb.Security.Authentication
{
    public interface IAmplaUserStore
    {
        AmplaUser GetUserByName(string userName);
        AmplaUser GetUserBySession(string session);
        void StoreUser(AmplaUser user);
        void RemoveUser(AmplaUser user);
    }
}