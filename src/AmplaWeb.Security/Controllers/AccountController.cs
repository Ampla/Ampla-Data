using System.Web.Mvc;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Security.Authentication;
using AmplaWeb.Security.Membership;
using AmplaWeb.Security.Models;

namespace AmplaWeb.Security.Controllers
{
    public class AccountController : BootstrapBaseController
    {
        private readonly IAmplaUserService amplaUserService;
        private readonly IFormsAuthenticationService formsAuthenticationService;

        public AccountController(IAmplaUserService amplaUserService, IFormsAuthenticationService formsAuthenticationService)
        {
            this.amplaUserService = amplaUserService;
            this.formsAuthenticationService = formsAuthenticationService;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string message;
                AmplaUser amplaUser = amplaUserService.Login(model.UserName, model.Password, out message);
                if (amplaUser != null)
                {
                    formsAuthenticationService.SignIn(amplaUser, model.RememberMe);

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
                && Url.IsLocalUrl(url) 
                && url.StartsWith("/") 
                && !url.StartsWith("//") 
                && !url.StartsWith("/\\");
        }

        //
        // GET: /Account/Logout
        public ActionResult Logout()
        {
            string user = HttpContext.User.Identity.Name;
            amplaUserService.Logout(user);
            formsAuthenticationService.SignOut();
            Information("Logout successful.");
            return RedirectToAction("Index", "Home");
        }

    }
}
