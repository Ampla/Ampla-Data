using System.Web.Security;

namespace AmplaWeb.Security.Authentication
{
    public class MembershipService : IMembershipService
    {
        private readonly MembershipProvider membershipProvider;

        public MembershipService(MembershipProvider membershipProvider)
        {
            this.membershipProvider = membershipProvider;
        }

        public bool ValidateUser(string userName, string password)
        {
            return membershipProvider.ValidateUser(userName, password);
        }
    }
}