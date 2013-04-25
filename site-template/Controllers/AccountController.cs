using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OAuth2.Client;
using OAuth2.Models;
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
            IClient client = _authService.GetClient(provider);
            return Redirect(client.GetLoginLinkUri(provider));
            //return new OAuthRequestActionResult(_authService.GetSecurityManager(provider), Url.Action("RequestAuthCallback"));
        }

        [HttpPost]
        public ActionResult AddAuth(string provider)
        {
            
            Session[SessionKeys.IsAddingAuth] = true;
            IClient client = _authService.GetClient( provider);

            return Redirect(client.GetLoginLinkUri(provider));
        }

        [AllowAnonymous]
        public ActionResult RequestAuthCallback()
        {
            if (Request.IsAuthenticated && !Session.Bool(SessionKeys.IsAddingAuth))
            {
                return View("Login");
            }

            IClient client = _authService.GetClient(Request.QueryString["state"]);
            try
            {
                UserInfo userInfo = client.GetUserInfo(Request.QueryString);
                if (Request.IsAuthenticated && Session.TestAndRemove(SessionKeys.IsAddingAuth))
                {
                    _authService.AddAuthToCurrentProfile(userInfo);
                    Session[SessionKeys.AddAuthSuccess] = true;
                    return RedirectToAction("Edit");

                }
                else
                {
                    LoginResponse loginResponse = _authService.Login(userInfo);
                    if (loginResponse.NewProfileCreated)
                    {
                        Session[SessionKeys.CreateProfile] = true;
                        return RedirectToAction("Edit");
                    }
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (ApplicationException ae)
            {
                //this means that auth didn't happen or there was some error.  User denied the auth or something.
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
