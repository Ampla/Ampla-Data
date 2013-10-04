namespace AmplaWeb.Security.Authentication
{
    /// <summary>
    /// Ampla user store that will manage Ampla user logins
    /// </summary>
    /// <remarks>
    ///     SessionId will be unique for a user
    ///     A userName may be referenced multiple times by different sessions.
    /// </remarks>
    public interface IAmplaUserStore
    {
        AmplaUser GetUserByName(string userName);
        AmplaUser GetUserBySession(string session);
        void StoreUser(AmplaUser user);
        void RemoveUser(AmplaUser user);
    }
}