using System.Collections.Generic;

namespace AmplaData.Security.Authentication
{
    public class AmplaUserStore : IAmplaUserStore
    {
        private readonly Dictionary<string, AmplaUser> validatedUserDictionary = new Dictionary<string, AmplaUser>();
        private readonly Dictionary<string, AmplaUser> validatedSessionDictionary = new Dictionary<string, AmplaUser>();
        private readonly object dictionaryLock = new object();

        public AmplaUser GetUserByName(string userName)
        {
            string lowerUser = userName.ToLower();
            AmplaUser user;
            lock (dictionaryLock)
            {
                validatedUserDictionary.TryGetValue(lowerUser, out user);
            }
            return user;
        }

        public AmplaUser GetUserBySession(string session)
        {
            AmplaUser user;
            lock (dictionaryLock)
            {
                validatedSessionDictionary.TryGetValue(session, out user);
            }
            return user;
        }

        public void StoreUser(AmplaUser user)
        {
            string lowerName = user.UserName.ToLower();
            string sessionId = user.Session;
            lock (dictionaryLock)
            {
                validatedUserDictionary[lowerName] = user;
                validatedSessionDictionary[sessionId] = user;
            }
        }

        public void RemoveUser(AmplaUser user)
        {
            if (user != null)
            {
                string lowerName = user.UserName.ToLower();
                string sessionId = user.Session;
                lock (dictionaryLock)
                {
                    validatedUserDictionary.Remove(lowerName);
                    validatedSessionDictionary.Remove(sessionId);
                }
            }
        }

    }
}