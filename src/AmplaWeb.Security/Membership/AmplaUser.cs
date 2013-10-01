using System;
using System.Web.Security;

namespace AmplaWeb.Security.Membership
{
    /// <summary>
    ///     Logged on Ampla User that represents the username, and Session
    /// </summary>
    public class AmplaUser : MembershipUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaUser"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="session">The session.</param>
        public AmplaUser(string userName, string session) : base(
            typeof(AmplaMembershipProvider).Name, 
            userName, 
            session, 
            null, 
            null, 
            null, 
            true, 
            false,
            DateTime.MinValue, 
            DateTime.MinValue, 
            DateTime.MinValue, 
            DateTime.MinValue, 
            DateTime.MinValue)
        {
            Session = session;
        }

        /// <summary>
        /// Gets the Ampla session for the current user
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        public string Session
        {
            get;
            private set;
        }

    }
}