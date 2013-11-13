using System;

namespace AmplaData.Web.Authentication
{
    public interface IAmplaUser
    {
        string UserName { get; }
        string Session { get; }
        string LoginType { get; }
        bool RememberToLogout { get; }

        DateTime LoginTime { get; }
        DateTime LastActivity { get; }
    }
}