using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using FluentValidation;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.Web.Mvc.FilterBindingSyntax;
using Raven.Client;
using matthewcv.common.Service;

namespace SiteTemplate.App_Start
{
    public class AuthenticationModule:NinjectModule
    {
        public override void Load()
        {
            //Bind<IOpenAuthDataProvider>().To<OAuthDataProvider>().InRequestScope();
            Bind<IAuthenticationService, IOpenAuthDataProvider>().ToMethod(CreateAuthContext).InRequestScope();
        }

        private AuthenticationService CreateAuthContext(IContext arg)
        {
            return new AuthenticationService(arg.Kernel.Get<HttpContextBase>(),arg.Kernel.Get<IDocumentSession>())
                .AddClient(new GoogleOpenIdClient())
                .AddClient(new FacebookClient(appId: "620392617974092", appSecret: "c1e39b8944a740ec074ac348316ed337"))
                ;

        }
    }
}