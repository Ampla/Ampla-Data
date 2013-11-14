using System;
using System.Security.Principal;
using AmplaData.AmplaSecurity2007;

namespace AmplaData.Web.Authentication
{
    /// <summary>
    ///     Ampla User Service that provides access to Ampla logins
    /// </summary>
    public class AmplaUserService : IAmplaUserService
    {
        private readonly ISecurityWebServiceClient securityWebService;
        private readonly IAmplaUserStore amplaUserStore;
       

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaUserService" /> class.
        /// </summary>
        /// <param name="securityWebService">The security web service.</param>
        /// <param name="amplaUserStore">The ampla user store.</param>
        public AmplaUserService(ISecurityWebServiceClient securityWebService, IAmplaUserStore amplaUserStore)
        {
            this.securityWebService = securityWebService;
            this.amplaUserStore = amplaUserStore;
        }

        /// <summary>
        /// Login an Ampla users using username and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public AmplaUser SimpleLogin(string userName, string password, out string message)
        {
            message = null;
            AmplaUser user = amplaUserStore.GetUserByName(userName);

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
                    user = new AmplaUser(response.Session.User, response.Session.SessionID, true, "Username/Password");
                    amplaUserStore.StoreUser(user);
                }

                if (user == null)
                {
                    message = exception.Message;
                }
            }
        
            return user;
        }

        /// <summary>
        /// Login an Ampla user using session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public AmplaUser SessionLogin(string session, out string message)
        {
            message = null;
            AmplaUser user = amplaUserStore.GetUserBySession(session);

            if (user != null)
            {
                user = Renew(user);
            }

            if (user == null)
            {
                RenewSessionRequest request = new RenewSessionRequest { Session = new Session { SessionID = session, User = ""} };

                Exception exception;
                RenewSessionResponse response = CatchExceptions(() => securityWebService.RenewSession(request), out exception);
                if (response != null)
                {
                    user = new AmplaUser(response.Session.User, response.Session.SessionID, false, "AmplaSession");
                    amplaUserStore.StoreUser(user);
                }
                if (user == null)
                {
                    message = exception.Message;
                }
            }

            return user;
        }

        public AmplaUser IntegratedLogin(out string message)
        {
            message = null;
            AmplaUser user = FindCurrentUser();

            if (user != null)
            {
                user = Renew(user);
            }

            if (user == null)
            {
                CreateSessionRequest request = new CreateSessionRequest();

                Exception exception;
                CreateSessionResponse response = CatchExceptions(() => securityWebService.CreateSession(request), out exception);
                if (response != null)
                {
                    user = new AmplaUser(response.Session.User, response.Session.SessionID, true, "Integrated");
                    amplaUserStore.StoreUser(user);
                }

                if (user == null)
                {
                    message = exception.Message;
                }
            }

            return user;
        }

        private AmplaUser FindCurrentUser()
        {
            IIdentity identity = WindowsIdentity.GetCurrent();

            return identity != null ? amplaUserStore.GetUserByName(identity.Name) : null;
        }

        /// <summary>
        /// Renew the session
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public AmplaUser RenewSession(string session)
        {
            AmplaUser user = amplaUserStore.GetUserBySession(session);

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
            AmplaUser user = amplaUserStore.GetUserByName(userName);
            if (user != null)
            {
                amplaUserStore.RemoveUser(user);

                if (user.RememberToLogout)
                {
                    ReleaseSessionRequest request = new ReleaseSessionRequest
                        {
                            Session = new Session {User = user.UserName, SessionID = user.Session}
                        };
                    Exception exception;
                    CatchExceptions(() => securityWebService.ReleaseSession(request), out exception);
                }
            }
        }

        private AmplaUser Renew(AmplaUser user)
        {
            AmplaUser renewedUser = null;
            if (user != null)
            {
                RenewSessionRequest request = new RenewSessionRequest { Session = new Session { User = user.UserName, SessionID = user.Session}};

                Exception exception;
                RenewSessionResponse response = CatchExceptions(() => securityWebService.RenewSession(request), out exception);
                if (response != null)
                {
                    renewedUser = user;
                    renewedUser.UpdateActivityDate();
                }
                else
                {
                    amplaUserStore.RemoveUser(user);
                }
            }
            return renewedUser;
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

    }
}