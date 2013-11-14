
using AmplaData.Adapters;
using AmplaData.Web.Authentication.Forms;
using AmplaData.Web.Sessions;

namespace AmplaData.Web.Authentication
{
    public class RenewSessionAdapter<TModel> : RepositoryAdapter<TModel>
    {
        private readonly IAmplaUserService userService;
        private readonly IFormsAuthenticationService formsAuthenticationService;
        private readonly IAmplaSessionStorage sessionStorage;
        
        public RenewSessionAdapter(IRepository<TModel> repository, 
                                   IAmplaUserService userService,
                                   IFormsAuthenticationService formsAuthenticationService,
                                   IAmplaSessionStorage sessionStorage) : base(repository)
        {
            this.userService = userService;
            this.formsAuthenticationService = formsAuthenticationService;
            this.sessionStorage = sessionStorage;
        }

        protected override void Adapt()
        {
            string session = sessionStorage.GetAmplaSession();
        
            if (!string.IsNullOrEmpty(session))
            {
                if (userService.RenewSession(session) == null)
                {
                    formsAuthenticationService.SessionExpired();
                }
            }
        }
    }
}