using System;
using System.Web.Security;
using AmplaWeb.Data.AmplaSecurity2007;

namespace AmplaWeb.Data.Membership
{
    public class AmplaMembershipProvider : ReadOnlyMembershipProvider
    {
        private readonly ISecurityWebServiceClient securityWebServiceClient;

        public AmplaMembershipProvider(ISecurityWebServiceClient securityWebServiceClient)
        {
            this.securityWebServiceClient = securityWebServiceClient;
        }

        public override bool ValidateUser(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            string session = Convert.ToString(providerUserKey);
            RenewSessionRequest request = new RenewSessionRequest {Session = new Session {SessionID = session}};
            
            RenewSessionResponse response = securityWebServiceClient.RenewSession(request);

            return new AmplaUser(response.Session.User, response.Session.SessionID);
        }
        
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new System.NotImplementedException();
        }
    }
}