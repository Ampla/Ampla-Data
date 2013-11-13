using System;
using System.Collections.Generic;

namespace AmplaData.Data.AmplaSecurity2007
{
    public class SimpleSecurityWebServiceClient : ISecurityWebServiceClient
    {
        public SimpleSecurityWebServiceClient(params string[] users)
        {
            possibleUsers = new List<string>(users ?? new string[0]).AsReadOnly();
            ValidatePasswordFunc = (s => s == "password");

            sessions = new List<SimpleSession>();
        }

        private readonly List<SimpleSession> sessions;

        private readonly IList<string> possibleUsers;
        
        public Func<string, bool> ValidatePasswordFunc { get; set; }

        /// <summary>
        /// Adds the existing session.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public void AddExistingSession(string userName)
        {
            SimpleSession existingSession = new SimpleSession(userName);
            sessions.Add(existingSession);
        }

        public List<SimpleSession> Sessions
        {
            get
            {
                return new List<SimpleSession>(sessions.AsReadOnly());
            }
        }

        public CreateSessionResponse CreateSession(CreateSessionRequest request)
        {
            string userName = request.Username;
            string password = request.Password;

            if (possibleUsers.Contains(userName))
            {
                bool isValid = ValidatePasswordFunc(password);
                if (isValid)
                {
                    SimpleSession session = sessions.Find(s => s.UserName == userName);
                    if (session == null)
                    {
                        session = new SimpleSession(userName);
                        sessions.Add(session);
                    }
                    else
                    {
                       if (!session.IsValid())
                       {
                           sessions.Remove(session);
                           session = new SimpleSession(userName);
                           sessions.Add(session);
                       }
                    }
                    session.Login();
                    return new CreateSessionResponse {Session = session.GetSession()};
                }
            }
            throw new InvalidOperationException("Unable to login with username and password");
        }

        public RenewSessionResponse RenewSession(RenewSessionRequest request)
        {
            string userName = request.Session.User;
            string sessionId = request.Session.SessionID;

            SimpleSession session = sessions.Find(s => s.SessionId == sessionId) ??
                                    sessions.Find(s => s.UserName == userName);
            if (session != null)
            {
                return new RenewSessionResponse { Session = session.GetSession() };
            }
            throw new InvalidOperationException("Unable to find user with session");
        }

        public ReleaseSessionResponse ReleaseSession(ReleaseSessionRequest request)
        {
            string userName = request.Session.User;
            string sessionId = request.Session.SessionID;
            SimpleSession session = sessions.Find(s => s.SessionId == sessionId) ??
                                    sessions.Find(s => s.UserName == userName);
            if (session != null)
            {
                session.Logout();
                if (!session.IsValid())
                {
                    sessions.Remove(session);
                }
            }
            return new ReleaseSessionResponse();
        }
    }
}