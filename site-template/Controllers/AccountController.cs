using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.OpenId.RelyingParty;
using Raven.Client;
using matthewcv.common.Entity;
using matthewcv.common.Infrastructure;
using matthewcv.common.Service;

namespace SiteTemplate.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public class SessionKeys
        {
            public const string CreateProfile = "CreateProfile";

            public const string IsAddingAuth = "IsAddingAuth";
            public const string AddAuthSuccess = "AddAuthSuccess";
        }
        private readonly IAuthenticationService _authService;

        public AccountController(IAuthenticationService authService)
        {
            _authService = authService;
            
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(_authService.OAuthClients);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RequestAuth(string provider)
        {
            if (Request.IsAuthenticated )
            {
                return View("Login");
            }
            return new OAuthRequestActionResult(_authService.GetSecurityManager(provider), Url.Action("RequestAuthCallback"));
        }

        [HttpPost]
        public ActionResult AddAuth(string provider)
        {
            
            Session[SessionKeys.IsAddingAuth] = true;
            return new OAuthRequestActionResult(_authService.GetSecurityManager(provider), Url.Action("RequestAuthCallback"));
        }

        [AllowAnonymous]
        public ActionResult RequestAuthCallback()
        {
            if (Request.IsAuthenticated && !Session.Bool(SessionKeys.IsAddingAuth))
            {
                return View("Login");
            }
            

            string providerName = OpenAuthSecurityManager.GetProviderName(HttpContext);
            OpenAuthSecurityManager m = _authService.GetSecurityManager(providerName);
            AuthenticationResult verifyAuthentication = m.VerifyAuthentication(Url.Action("RequestAuthCallback"));

            if (verifyAuthentication.IsSuccessful)
            {
                if (Request.IsAuthenticated && Session.TestAndRemove(SessionKeys.IsAddingAuth))
                {
                    _authService.AddAuthToCurrentProfile(verifyAuthentication);
                    Session[SessionKeys.AddAuthSuccess] = true;
                    return RedirectToAction("Edit");

                }
                else
                {
                    LoginResponse loginResponse = _authService.Login(verifyAuthentication);
                    if (loginResponse.NewProfileCreated)
                    {
                        Session[SessionKeys.CreateProfile] = true;
                        return RedirectToAction("Edit");
                    }
                    return RedirectToAction("Index", "Home");
                }
            }


            return RedirectToAction("Login");
        }
        
        public ActionResult Edit()
        {
            
            if (Session.TestAndRemove(SessionKeys.CreateProfile))
            {
                ViewBag.CreateProfile = true;
            }

            if (Session.TestAndRemove(SessionKeys.AddAuthSuccess))
            {
                ViewBag.AddAuthSuccess = true;
            }
            ViewBag.OtherOauths = _authService.OAuthClients.Where(c => !_authService.CurrentProfile.OAuthIdentities.Select(i => i.Provider).Contains(c.ProviderName)).ToList();
            return View(_authService.CurrentProfile);
        }

        [HttpPost]
        public ActionResult Edit(Profile profile)
        {
            if (ModelState.IsValid)
            {
                _authService.UpdateCurrentProfile(profile);
                return SaveSuccess();
            }
            return ModelValidationErrors();
        }

        private ActionResult SaveSuccess()
        {
            return Json(new {SaveSuccess = true});
        }

        private ActionResult ModelValidationErrors()
        {
            return Json(new { SaveSuccess = false, Errors = ModelState.Where(s => s.Value.Errors.Any()).SelectMany(s => s.Value.Errors.Select(e => e.ErrorMessage)) });
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
