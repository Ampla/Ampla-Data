using System.Web.Mvc;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Data.Membership;
using AmplaWeb.Security.Authentication;
using AmplaWeb.Security.Membership;
using AmplaWeb.Security.Models;

namespace AmplaWeb.Security.Controllers
{
    public class AccountController : BootstrapBaseController
    {
        private readonly IMembershipService membershipService;
        private readonly IFormsAuthenticationService formsAuthenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="membershipService">The membership service.</param>
        /// <param name="formsAuthenticationService">The forms authentication service.</param>
        public AccountController(IMembershipService membershipService, IFormsAuthenticationService formsAuthenticationService)
        {
            this.membershipService = membershipService;
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
                if (membershipService.ValidateUser(model.UserName, model.Password))
                {
                    formsAuthenticationService.SignIn(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        Information("Login successful.");
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Logout
        public ActionResult Logout()
        {
            formsAuthenticationService.SignOut();
            Information("Logout successful.");
            return RedirectToAction("Index", "Home");
        }

    }
}
