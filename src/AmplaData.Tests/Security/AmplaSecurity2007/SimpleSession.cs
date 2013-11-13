using System;
using AmplaData.Data.AmplaSecurity2007;

namespace AmplaData.Security.AmplaSecurity2007
{
    public class SimpleSession
    {
        public SimpleSession(string userName)
        {
            UserName = userName;
            SessionId = Guid.NewGuid().ToString();
            Count = 0;
        }

        public string UserName { get; private set; }
        public string SessionId { get; private set; }

        public int Count { get; private set; }

        public void Login()
        {
            Count++;
        }

        public void Logout()
        {
            Count--;
        }

        public bool IsValid()
        {
            return Count > 0;
        }

        public Session GetSession()
        {
            return new Session {User = UserName, SessionID = SessionId};
        }
    }
}