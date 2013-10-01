using System;
using System.Web.Security;
using AmplaWeb.Security.Authentication;

namespace AmplaWeb.Security.Membership
{
    public class AmplaMembershipProvider : ReadOnlyMembershipProvider
    {
        private readonly IAmplaUserService amplaUserService;

        public AmplaMembershipProvider() : this(new AmplaUserService())
        {
        }

        public AmplaMembershipProvider(IAmplaUserService amplaUserService)
        {
            this.amplaUserService = amplaUserService;
        }

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <returns>
        /// true if the specified username and password are valid; otherwise, false.
        /// </returns>
        public override bool ValidateUser(string username, string password)
        {
            string message;
            AmplaUser amplaUser = amplaUserService.Login(username, password, out message);
            return amplaUser != null;
        }

        /// <summary>
        /// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the specified user's information from the data source.
        /// </returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            string session = Convert.ToString(providerUserKey);
            AmplaUser amplaUser = amplaUserService.RenewSession(session);
            return amplaUser;
        }
        
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            AmplaUser amplaUser = amplaUserService.GetLoggedInUser(username);
            return amplaUser;
        }
    }
}