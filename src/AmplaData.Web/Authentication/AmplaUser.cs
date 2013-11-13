using System;

namespace AmplaData.Web.Authentication
{
    /// <summary>
    ///     Logged on Ampla User that represents the Ampla User
    /// </summary>
    public class AmplaUser : IAmplaUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaUser" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="session">The session.</param>
        /// <param name="rememberToLogout">if set to <c>true</c> [remember automatic logout].</param>
        /// <param name="loginType">Type of the login.</param>
        public AmplaUser(string userName, string session, bool rememberToLogout, string loginType)
        {
            UserName = userName;
            Session = session;
            RememberToLogout = rememberToLogout;
            LoginType = loginType;
            LoginTime = DateTime.Now;
            UpdateActivityDate();
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the Ampla session for the current user
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        public string Session { get; private set; }

        /// <summary>
        /// Gets the type of the login.
        /// </summary>
        /// <value>
        /// The type of the login.
        /// </value>
        public string LoginType { get; private set; }

        /// <summary>
        /// Should the user be logged out when finished
        /// </summary>
        public bool RememberToLogout { get; set; }

        /// <summary>
        /// Gets the login time.
        /// </summary>
        /// <value>
        /// The login time.
        /// </value>
        public DateTime LoginTime { get; private set; }

        /// <summary>
        /// Gets the last activity.
        /// </summary>
        /// <value>
        /// The last activity.
        /// </value>
        public DateTime LastActivity { get; private set; }

        /// <summary>
        /// Updates the Last activity date.
        /// </summary>
        public void UpdateActivityDate()
        {
            LastActivity = DateTime.Now;
        }
    }
}