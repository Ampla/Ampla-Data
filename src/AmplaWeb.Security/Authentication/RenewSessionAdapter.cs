using System.Web.Security;
using AmplaWeb.Data;

namespace AmplaWeb.Security.Authentication
{
    public class RenewSessionAdapter<TModel> : Data.Adapters.RepositoryAdapter<TModel>
    {
        private readonly IAmplaUserService userService;
        private readonly IFormsAuthenticationService formsAuthenticationService;

        public RenewSessionAdapter(IRepository<TModel> repository, IAmplaUserService userService, IFormsAuthenticationService formsAuthenticationService) : base(repository)
        {
            this.userService = userService;
            this.formsAuthenticationService = formsAuthenticationService;
        }

        protected override void Adapt()
        {
            FormsAuthenticationTicket ticket = formsAuthenticationService.GetUserTicket();
            string session = ticket.UserData;
            
            if (userService.RenewSession(session) == null)
            {
                formsAuthenticationService.SignOut();
            }
        }
    }
}