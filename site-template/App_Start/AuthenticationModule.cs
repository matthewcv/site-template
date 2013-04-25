using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.Extensions.Conventions;
using Ninject.Web.Mvc.FilterBindingSyntax;
using OAuth2.Client;
using OAuth2.Client.Impl;
using OAuth2.Configuration;
using Raven.Client;
using matthewcv.common.Service;

namespace SiteTemplate.App_Start
{
    public class AuthenticationModule:NinjectModule
    {
        public override void Load()
        {
            Bind<IAuthenticationService>().To<AuthenticationService>().InRequestScope();

            Kernel.Bind(x => x.FromAssemblyContaining<IClient>()
                .Select(t => t != typeof(OAuth2ConfigurationSection))
                .BindAllInterfaces()
                .Configure(c => c.InRequestScope()));

            Bind<IOAuth2Configuration>()
                .ToMethod(c => c.Kernel.Get<IConfigurationManager>().GetConfigSection<IOAuth2Configuration>("oauth2"))
                .InSingletonScope();

        }

    }
}