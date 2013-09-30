using System;
using System.Web.Security;

using AmplaWeb.Security.AmplaSecurity2007;

namespace AmplaWeb.Security.Membership
{
    public class AmplaMembershipProvider : ReadOnlyMembershipProvider
    {
        private readonly ISecurityWebServiceClient securityWebServiceClient;

        public AmplaMembershipProvider() : this(new SecurityWebServiceFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaMembershipProvider"/> class.
        /// </summary>
        /// <param name="securityWebServiceClient">The security web service client.</param>
        public AmplaMembershipProvider(ISecurityWebServiceClient securityWebServiceClient)
        {
            this.securityWebServiceClient = securityWebServiceClient;
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
            CreateSessionRequest request = new CreateSessionRequest {Username = username, Password = password};

            CreateSessionResponse response = securityWebServiceClient.CreateSession(request);
            return response != null;
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
            RenewSessionRequest request = new RenewSessionRequest {Session = new Session {SessionID = session}};
            
            RenewSessionResponse response = securityWebServiceClient.RenewSession(request);

            return new AmplaUser(response.Session.User, response.Session.SessionID);
        }
        
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
    }
}