using System.Web.Mvc;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Data.Sessions;
using AmplaWeb.Security.Authentication;
using AmplaWeb.Security.Authentication.Forms;
using AmplaWeb.Security.Models;

namespace AmplaWeb.Security.Controllers
{
    /// <summary>
    /// Account Controller to provide access 
    /// </summary>
    public class AccountController : BootstrapBaseController
    {
        private readonly IAmplaUserService amplaUserService;
        private readonly IFormsAuthenticationService formsAuthenticationService;
        private readonly IAmplaSessionStorage amplaSessionStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        /// <param name="amplaUserService">The ampla user service.</param>
        /// <param name="formsAuthenticationService">The forms authentication service.</param>
        /// <param name="amplaSessionStorage">The ampla session storage.</param>
        public AccountController(IAmplaUserService amplaUserService, IFormsAuthenticationService formsAuthenticationService, IAmplaSessionStorage amplaSessionStorage)
        {
            this.amplaUserService = amplaUserService;
            this.formsAuthenticationService = formsAuthenticationService;
            this.amplaSessionStorage = amplaSessionStorage;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // GET: /Account/User
        [HttpGet]
        [ActionName("User")]
        [Authorize]
        public ActionResult CurrentUser()
        {
            string session = amplaSessionStorage.GetAmplaSession();

            AmplaUser user = amplaUserService.RenewSession(session);

            UserModel model = new UserModel
                {
                    UserName = user.UserName,
                    Session = user.Session,
                    Started = user.LoginTime,
                    LastActivity = user.LastActivity,
                    LoginType = user.LoginType
                };
            
            return View("CurrentUser", model);
        }
        
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginIntegrated(string returnUrl)
        {
            IntegratedLoginModel model = new IntegratedLoginModel {UseIntegratedSecurity = true};
            
            if (ModelState.IsValid)
            {
                string message;
                if (!model.UseIntegratedSecurity)
                {
                    return RedirectToAction("Login");
                }

                AmplaUser amplaUser = amplaUserService.IntegratedLogin(out message);
                if (amplaUser != null)
                {
                    amplaSessionStorage.SetAmplaSession(amplaUser.Session);
                    formsAuthenticationService.StoreUserTicket(amplaUser, model.RememberMe);

                    if (UrlIsLocal(returnUrl))
                    {
                        Information("Login successful.");
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                Error(message);
                ModelState.AddModelError("", message);
            }

            // If we got this far, something failed, redisplay form
            return View("Login");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(SimpleLoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string message;
                AmplaUser amplaUser = amplaUserService.SimpleLogin(model.UserName, model.Password, out message);
                if (amplaUser != null)
                {
                    amplaSessionStorage.SetAmplaSession(amplaUser.Session);

                    formsAuthenticationService.StoreUserTicket(amplaUser, model.RememberMe);

                    if (UrlIsLocal(returnUrl))
                    {
                        Information("Login successful.");
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                Error(message);
                ModelState.AddModelError("", message);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private bool UrlIsLocal(string url)
        {
            return 
                !string.IsNullOrEmpty(url) 
                && (Url == null || Url.IsLocalUrl(url)) 
                && url.StartsWith("/") 
                && !url.StartsWith("//") 
                && !url.StartsWith("/\\");
        }

        //
        // POST: /Account/Logout
        [HttpPost]
        public ActionResult Logout()
        {
            var ticket = formsAuthenticationService.GetUserTicket();
            if (ticket != null)
            {
                string user = ticket.Name;
                amplaUserService.Logout(user);
                Information("Logout successful.");
            }
            amplaSessionStorage.SetAmplaSession(null);
            formsAuthenticationService.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}
