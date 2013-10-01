using System;
using System.Collections.Generic;
using AmplaWeb.Security.AmplaSecurity2007;
using AmplaWeb.Security.Membership;

namespace AmplaWeb.Security.Authentication
{
    /// <summary>
    ///     Ampla User Service that provides access to Ampla logins
    /// </summary>
    public class AmplaUserService : IAmplaUserService
    {
        private readonly ISecurityWebServiceClient securityWebService;
        private readonly Dictionary<string, AmplaUser> validatedUserDictionary = new Dictionary<string, AmplaUser>();
        private readonly Dictionary<string, AmplaUser> validatedSessionDictionary = new Dictionary<string, AmplaUser>();
        private readonly object dictionaryLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaUserService"/> class.
        /// </summary>
        public AmplaUserService() : this(new SecurityWebServiceFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaUserService"/> class.
        /// </summary>
        /// <param name="securityWebService">The security web service.</param>
        public AmplaUserService(ISecurityWebServiceClient securityWebService)
        {
            this.securityWebService = securityWebService;
        }

        /// <summary>
        /// Login an Ampla users using username and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public AmplaUser Login(string userName, string password, out string message)
        {
            message = null;
            AmplaUser user = FindUserByName(userName);

            if (user != null)
            {
                user = Renew(user);
            }

            if (user == null)
            {
                CreateSessionRequest request = new CreateSessionRequest {Username = userName, Password = password};

                Exception exception;
                CreateSessionResponse response = CatchExceptions(() => securityWebService.CreateSession(request), out exception);
                if (response != null)
                {
                    user = new AmplaUser(response.Session.User, response.Session.SessionID);
                    StoreUser(user);
                }

                if (user == null)
                {
                    message = exception.Message;
                }
            }
        
            return user;
        }

        /// <summary>
        /// Renew the session
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public AmplaUser RenewSession(string session)
        {
            AmplaUser user = FindUserBySession(session);

            if (user != null)
            {
                user = Renew(user);
            }

            return user;
        }


        /// <summary>
        /// Gets the logged in user with name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public AmplaUser GetLoggedInUser(string userName)
        {
            AmplaUser user = FindUserByName(userName);
            if (user != null)
            {
                user = Renew(user);
            }

            return user;
        }

        /// <summary>
        /// Logout the user
        /// </summary>
        /// <param name="userName"></param>
        public void Logout(string userName)
        {
            AmplaUser user = FindUserByName(userName);
            if (user != null)
            {
                RemoveUser(user);

                ReleaseSessionRequest request = new ReleaseSessionRequest {
                        Session = new Session {User = user.UserName, SessionID = user.Session}
                    };
                Exception exception;
                CatchExceptions(() => securityWebService.ReleaseSession(request), out exception);
            }
        }

        /// <summary>
        /// Gets an array of currently logged in users
        /// </summary>
        /// <returns></returns>
        public string[] GetUsers()
        {
            List<string> users;
            lock (dictionaryLock)
            {
                users = new List<string>(validatedUserDictionary.Keys);
            }
            return users.ToArray();
        }


        private AmplaUser Renew(AmplaUser user)
        {
            if (user != null)
            {
                RenewSessionRequest request = new RenewSessionRequest { Session = new Session { User = user.UserName, SessionID = user.Session}};

                Exception exception;
                RenewSessionResponse response = CatchExceptions(() => securityWebService.RenewSession(request), out exception);
                if (response != null)
                {
                    user = new AmplaUser(response.Session.User, response.Session.SessionID);
                    StoreUser(user);
                }
                else
                {
                    RemoveUser(user);
                    user = null;
                }
            }
            return user;
        }

        private T CatchExceptions<T>(Func<T> func, out Exception exception)
        {
            try
            {
                exception = null;
                return func();
            }
            catch (Exception ex)
            {
                exception = ex;
                return default(T);
            }
        }

        private AmplaUser FindUserByName(string userName)
        {
            string lowerUser = userName.ToLower();
            AmplaUser user;
            lock (dictionaryLock)
            {
                validatedUserDictionary.TryGetValue(lowerUser, out user);
            }
            return user;
        }

        private AmplaUser FindUserBySession(string session)
        {
            AmplaUser user;
            lock (dictionaryLock)
            {
                validatedSessionDictionary.TryGetValue(session, out user);
            }
            return user;
        }
        
        private void StoreUser(AmplaUser user)
        {
            string lowerName = user.UserName.ToLower();
            string sessionId = user.Session;
            lock (dictionaryLock)
            {
                validatedUserDictionary[lowerName] = user;
                validatedSessionDictionary[sessionId] = user;
            }
        }

        private void RemoveUser(AmplaUser user)
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